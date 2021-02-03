using Microsoft.Extensions.Options;
using NetBlade.CrossCutting.ClientFactory.WCF.ConfigurationOption;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetBlade.CrossCutting.ClientFactory.WCF
{
    public class WCFServiceFactory : IDisposable
    {
        public WCFServiceFactory(IOptions<ServicesConfigurationOption> serviceOptions)
        {
            this.Services = new ConcurrentDictionary<Type, ServiceCache>();
            this.ServiceOptions = serviceOptions;
        }

        protected virtual IOptions<ServicesConfigurationOption> ServiceOptions { get; set; }

        protected virtual ConcurrentDictionary<Type, ServiceCache> Services { get; set; }

        public virtual void Dispose()
        {
            if (this.Services != null)
            {
                foreach (KeyValuePair<Type, ServiceCache> service in this.Services)
                {
                    service.Value.Dispose();
                }
            }

            this.Services = null;
            this.ServiceOptions = null;
        }

        public virtual TService GetService<TService>(Func<ICommunicationObject, OperationContextScope> fnCreateOperationContextScope = null)
        {
            return this.GetService<TService>(typeof(TService).Name, fnCreateOperationContextScope);
        }

        public virtual TService GetService<TService>(string serviceName, Func<ICommunicationObject, OperationContextScope> fnCreateOperationContextScope = null)
        {
            EndPointConfigurationOption endpoint = this.GetServiceAddress(serviceName);
            return this.GetService<TService>(endpoint, fnCreateOperationContextScope);
        }

        public virtual TService GetService<TService>(EndPointConfigurationOption endpoint, Func<ICommunicationObject, OperationContextScope> fnCreateOperationContextScope = null)
        {
            Binding binding = this.GetBinding(endpoint.Binding);
            EndpointAddress endpointAddress = new EndpointAddress(endpoint.Address);

            return this.GetService<TService>(binding, endpointAddress, fnCreateOperationContextScope);
        }

        public virtual TService GetService<TService>(Binding binding, EndpointAddress endpointAddress, Func<ICommunicationObject, OperationContextScope> fnCreateOperationContextScope = null)
        {
            fnCreateOperationContextScope = fnCreateOperationContextScope ?? this.CreateOperationContextScope;
            ServiceCache serviceCache = this.GetServiceCache<TService>(binding, endpointAddress);
            if (serviceCache.CommunicationObject.State == CommunicationState.Faulted)
            {
                serviceCache.Dispose();
                _ = this.Services.TryRemove(typeof(TService), out ServiceCache _);
                return this.GetService<TService>(binding, endpointAddress);
            }
            else
            {
                OperationContextScope scope = fnCreateOperationContextScope(serviceCache.CommunicationObject);
                if (scope != null)
                {
                    serviceCache.Scope?.Dispose();
                    serviceCache.Scope = scope;
                }

                return (TService)serviceCache.CommunicationObject;
            }
        }

        protected virtual OperationContextScope CreateOperationContextScope(ICommunicationObject service)
        {
            return null;
        }

        protected virtual Binding GetBinding(string bindingName)
        {
            Binding binding = null;
            if (this.ServiceOptions != null)
            {
                BindingConfigurationsOption config = this.ServiceOptions.Value.Bindings[bindingName];
                switch (config.Type.ToUpper())
                {
                    case "NETTCP":
                        NetTcpBinding netTcpBinding = new NetTcpBinding
                        {
                            CloseTimeout = new TimeSpan(0, 0, config.CloseTimeout ?? 1, 0, 0),
                            OpenTimeout = new TimeSpan(0, 0, config.OpenTimeout ?? 1, 0, 0),
                            ReceiveTimeout = new TimeSpan(0, 0, config.ReceiveTimeout ?? 1, 0, 0),
                            SendTimeout = new TimeSpan(0, 0, config.SendTimeout ?? 1, 0, 0),
                            MaxBufferSize = config.MaxBufferSize ?? 65536,
                            MaxBufferPoolSize = config.MaxBufferPoolSize ?? 524288,
                            MaxReceivedMessageSize = config.MaxReceivedMessageSize ?? 65536,
                            TransferMode = (TransferMode)(config.TransferMode ?? (int)TransferMode.Buffered),
                            ReaderQuotas = this.GetReaderQuotas(config)
                        };
                        switch (netTcpBinding.Security.Mode)
                        {
                            case SecurityMode.Message:
                            case SecurityMode.TransportWithMessageCredential:
                                netTcpBinding.Security = new NetTcpSecurity
                                {
                                    Mode = (SecurityMode)(config.Security.Mode ?? (int)SecurityMode.Transport),
                                    Message =
                                    {
                                        ClientCredentialType = (MessageCredentialType) (config.Security.MessageClientCredentialType ?? (int) MessageCredentialType.Windows)
                                    }
                                };
                                break;
                            case SecurityMode.Transport:
                            case SecurityMode.None:
                                netTcpBinding.Security = new NetTcpSecurity
                                {
                                    Mode = (SecurityMode)(config.Security.Mode ?? (int)SecurityMode.Transport),
                                    Transport =
                                    {
                                        ClientCredentialType = (TcpClientCredentialType) (config.Security.TransportClientCredentialType ?? (int) TcpClientCredentialType.Windows)
                                    }
                                };
                                break;
                        }

                        binding = netTcpBinding;
                        break;
                    case "BASICHTTP":
                        binding = new BasicHttpBinding
                        {
                            CloseTimeout = new TimeSpan(0, 0, config.CloseTimeout ?? 1, 0, 0),
                            OpenTimeout = new TimeSpan(0, 0, config.OpenTimeout ?? 1, 0, 0),
                            SendTimeout = new TimeSpan(0, 0, config.SendTimeout ?? 1, 0, 0),
                            MaxReceivedMessageSize = config.MaxReceivedMessageSize ?? 65536,
                            MaxBufferSize = config.MaxBufferSize ?? 65536,
                            MaxBufferPoolSize = config.MaxBufferPoolSize ?? 524288,
                            ReaderQuotas = this.GetReaderQuotas(config),
                            TransferMode = (TransferMode)(config.TransferMode ?? (int)TransferMode.Buffered),
                            Security = new BasicHttpSecurity
                            {
                                Mode = (BasicHttpSecurityMode)(config.Security.Mode ?? (int)BasicHttpSecurityMode.None),
                                Transport = new HttpTransportSecurity
                                {
                                    ClientCredentialType = (HttpClientCredentialType)(config.Security.TransportClientCredentialType ?? (int)TcpClientCredentialType.None)
                                }
                            }
                        };
                        break;
                }
            }

            return binding;
        }

        protected virtual XmlDictionaryReaderQuotas GetReaderQuotas(BindingConfigurationsOption config)
        {
            return new XmlDictionaryReaderQuotas
            {
                MaxArrayLength = config.ReaderQuotas.MaxArrayLength ?? 16384,
                MaxBytesPerRead = config.ReaderQuotas.MaxBytesPerRead ?? 4096,
                MaxDepth = config.ReaderQuotas.MaxDepth ?? 32,
                MaxNameTableCharCount = config.ReaderQuotas.MaxNameTableCharCount ?? 16384,
                MaxStringContentLength = config.ReaderQuotas.MaxStringContentLength ?? 8192
            };
        }

        protected virtual EndPointConfigurationOption GetServiceAddress(string endpointName)
        {
            EndPointConfigurationOption address = null;
            if (this.ServiceOptions != null)
            {
                address = this.ServiceOptions.Value.EndPoints[endpointName];
            }

            return address;
        }

        protected virtual ServiceCache GetServiceCache<TService>(Binding binding, EndpointAddress endpointAddress)
        {
            return this.Services.GetOrAdd(typeof(TService), t => new ServiceCache((ICommunicationObject)new ChannelFactory<TService>(binding, endpointAddress).CreateChannel()));
        }

        protected class ServiceCache : IDisposable
        {
            public ServiceCache(ICommunicationObject communicationObject)
            {
                this.CommunicationObject = communicationObject;
            }

            public ICommunicationObject CommunicationObject { get; set; }

            public OperationContextScope Scope { get; set; }

            public void Dispose()
            {
                try
                {
                    if (this.CommunicationObject != null)
                    {
                        switch (this.CommunicationObject.State)
                        {
                            case CommunicationState.Created:
                            case CommunicationState.Faulted:
                                this.CommunicationObject.Abort();
                                break;
                            case CommunicationState.Opening:
                            case CommunicationState.Opened:
                                try
                                {
                                    this.CommunicationObject.Close();
                                    break;
                                }
                                catch
                                {
                                    this.CommunicationObject.Abort();
                                    break;
                                }
                        }
                    }

                    this.Scope?.Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
