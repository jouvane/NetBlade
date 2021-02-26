using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Movies.API.Filters;
using Movies.CrossCutting.Comum;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Querys;
using System;
using System.Collections.Generic;
using Xunit;

namespace Movies.Tests.UnitTest.API
{
    public class ComumResponseActionFilterAttributeTest : IClassFixture<ComumResponseActionFilterAttributeTest.Startup>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ComumResponseActionFilterAttribute _comumResponseActionFilterAttribute;

        public ComumResponseActionFilterAttributeTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
            this._comumResponseActionFilterAttribute = this._serviceProvider.GetRequiredService<ComumResponseActionFilterAttribute>();
        }

        [Fact]
        public void ParseExceptionComumResponseViewModelAggregateExceptionTest()
        {
            QueryResponse<int> response = new QueryResponse<int>(10);
            response.SetException(new AggregateException(new DomainException("Já existem os mesmos dados de")));

            BadRequestObjectResult actionResult = this._comumResponseActionFilterAttribute.ParseExceptionComumResponseViewModel(response) as BadRequestObjectResult;
            ComumResponseActionFilterAttributeTest.ValidateBadRequestObjectResultSingle(actionResult, "Já existem os mesmos dados de");
        }

        [Fact]
        public void ParseExceptionComumResponseViewModelAggregateException2Test()
        {
            QueryResponse<int> response = new QueryResponse<int>(10);
            response.SetException(new AggregateException(new AggregateException(new DomainException("Já existem os mesmos dados de"))));

            BadRequestObjectResult actionResult = this._comumResponseActionFilterAttribute.ParseExceptionComumResponseViewModel(response) as BadRequestObjectResult;
            ComumResponseActionFilterAttributeTest.ValidateBadRequestObjectResultSingle(actionResult, "Já existem os mesmos dados de");
        }

        [Fact]
        public void ParseExceptionComumResponseViewModelAggregateException3Test()
        {
            QueryResponse<int> response = new QueryResponse<int>(10);
            response.SetException(new AggregateException(new AggregateException(new AggregateException(new DomainException("Já existem os mesmos dados de")))));

            BadRequestObjectResult actionResult = this._comumResponseActionFilterAttribute.ParseExceptionComumResponseViewModel(response) as BadRequestObjectResult;
            ComumResponseActionFilterAttributeTest.ValidateBadRequestObjectResultSingle(actionResult, "Já existem os mesmos dados de");
        }

        [Fact]
        public void RegistroDuplicadoDomainExceptionTest()
        {
            QueryResponse<int> response = new QueryResponse<int>(10);
            response.SetException(new DomainException("Já existem os mesmos dados de"));

            BadRequestObjectResult actionResult = this._comumResponseActionFilterAttribute.ParseExceptionComumResponseViewModel(response) as BadRequestObjectResult;
            ComumResponseActionFilterAttributeTest.ValidateBadRequestObjectResultSingle(actionResult, "Já existem os mesmos dados de");
        }

        private static void ValidateBadRequestObjectResultSingle(BadRequestObjectResult actionResult, string msg)
        {
            Assert.NotNull(actionResult);
            IList<ValidationFailure> validationFailures = actionResult.Value as IList<ValidationFailure>;
            Assert.NotNull(validationFailures);
            Assert.Single(validationFailures);
            Assert.Equal(msg, validationFailures[0].ErrorMessage);
        }

        public class Startup : StartupBase
        {
            public override IServiceCollection OnConfigureServices(IServiceCollection services)
            {
                return services
                    .AddScoped<ComumResponseActionFilterAttribute>()
                    .AddScoped(_ => Mock.Of<IWebHostEnvironment>())
                    .AddScoped(_ => new TransactionID("TesteID"));
            }
        }
    }
}
