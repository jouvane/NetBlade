using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Movies.CrossCutting.Comum;
using NetBlade.Core;
using NetBlade.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Movies.API.Filters
{
    public class ComumResponseActionFilterAttribute : ActionFilterAttribute
    {
        private const string _erroResultMessager = "Ocorreu um erro inesperado. Tente novamente mais tarde. {0}";
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ComumResponseActionFilterAttribute> _logger;
        private readonly TransactionID _transactionID;

        public ComumResponseActionFilterAttribute(IWebHostEnvironment env, ILogger<ComumResponseActionFilterAttribute> logger, TransactionID transactionID)
        {
            this._env = env;
            this._logger = logger;
            this._transactionID = transactionID;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Type resultType = context?.Result?.GetType();
            PropertyInfo valuePropertyInfo = resultType?.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            object result = null;

            if (resultType != null && valuePropertyInfo != null)
            {
                result = valuePropertyInfo.GetValue(context.Result);
            }

            IResponse response;
            if (result is IResponse || context?.Result is IResponse)
            {
                response = result as IResponse ?? context.Result as IResponse;
                if (response.Success)
                {
                    context.Result = this.ParseSuccessComumResponseViewModel(response);
                }
                else
                {
                    context.Result = this.ParseExceptionComumResponseViewModel(response);
                }
            }

            base.OnResultExecuting(context);
        }

        public ActionResult ParseExceptionComumResponseViewModel(IResponse response)
        {
            ActionResult actionResult;
            Exception exception = response.Exception;

            if (exception is AggregateException aggEx)
            {
                exception = this.GetDomainExceptionByAggregateException(aggEx);
            }

            if (exception is DomainException)
            {
                if (exception is INotRollbackTransaction)
                {
                    actionResult = new OkObjectResult(exception?.Message);
                }
                else if (exception is ValidationResultDomainException exValidationResultDomainException)
                {
                    foreach (ValidationFailure item in exValidationResultDomainException.Validations)
                    {
                        item.AttemptedValue = null;
                        item.FormattedMessagePlaceholderValues = null;
                    }

                    actionResult = new BadRequestObjectResult(exValidationResultDomainException.Validations);
                }
                else if (exception is NotFoundDomainException exNotFoundDomainException)
                {
                    actionResult = new NotFoundObjectResult(exNotFoundDomainException.Message);
                }
                else
                {
                    actionResult = new BadRequestObjectResult(new List<ValidationFailure> { new ValidationFailure(exception.GetType().Name, exception.Message) });
                }
            }
            else
            {
                this._logger.LogError(exception, null);
                if (!this._env.IsDevelopment())
                {
                    string erroResultMessager = string.Format(ComumResponseActionFilterAttribute._erroResultMessager, this._transactionID.ID);
                    actionResult = new ObjectResult(new ValidationFailure("Status500InternalServerError", erroResultMessager))
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                else
                {
                    actionResult = new BadRequestObjectResult(new[] { new ValidationFailure(exception.GetType().Name, exception.Message) });
                }
            }

            return actionResult;
        }

        public ActionResult ParseSuccessComumResponseViewModel(IResponse response)
        {
            PropertyInfo dataProperty = response.GetType().GetProperty(nameof(IResponse<int>.Data), BindingFlags.Public | BindingFlags.Instance);
            if (dataProperty != null)
            {
                return new OkObjectResult(dataProperty.GetValue(response));
            }
            else
            {
                return new OkObjectResult(null);
            }
        }

        private Exception GetDomainExceptionByAggregateException(AggregateException aggEx)
        {
            Exception ex = null;
            if (aggEx.InnerExceptions.Count == 1)
            {
                if (aggEx.InnerExceptions[0] is AggregateException)
                {
                    ex = this.GetDomainExceptionByAggregateException(aggEx.InnerExceptions[0] as AggregateException);
                }

                if (ex is DomainException || aggEx.InnerExceptions[0] is DomainException)
                {
                    return aggEx.InnerExceptions[0] as DomainException ?? ex;
                }
            }
            else if (aggEx.InnerExceptions.Any(a => a is DomainException))
            {
                ex = new ValidationResultDomainException();
                aggEx.InnerExceptions.ToList().ForEach((Exception item) =>
                {
                    DomainException domainException = (DomainException)item;
                    ((ValidationResultDomainException)ex).AddValidationError(new ValidationFailure(domainException.GetType().ToString(), item.Message));
                });
            }

            return ex ?? aggEx;
        }
    }
}
