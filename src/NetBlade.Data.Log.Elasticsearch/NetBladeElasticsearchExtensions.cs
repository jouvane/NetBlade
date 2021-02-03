using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Linq;

namespace NetBlade.Data.Log.Elasticsearch
{
    public static class NetBladeElasticsearchExtensions
    {
        public static LoggerConfiguration NetBladeElasticsearch(this LoggerSinkConfiguration loggerSinkConfiguration, string nodeUris = "", string indexFormat = "", string username = "", string password = "", bool renderMessage = true, bool autoRegisterTemplate = true)
        {
            Uri[] uris = nodeUris.Split(';').Select(s => new Uri(s)).ToArray();
            return loggerSinkConfiguration.Elasticsearch(new ElasticsearchSinkOptions(uris)
            {
                AutoRegisterTemplate = autoRegisterTemplate,
                IndexFormat = indexFormat,
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: renderMessage),
                ModifyConnectionSettings = connectionConfiguration =>
                {
                    return connectionConfiguration
                    .ServerCertificateValidationCallback((a, b, c, d) => true)
                    .BasicAuthentication(username, password);
                }
            });
        }
    }
}
