using FluentValidation;
using NetBlade.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Core.Querys
{
    public abstract class QueryHandlerBase<TQuery, TQueryResponse> : QueryHandlerBase
        where TQuery : IQuery
        where TQueryResponse : IQueryResponse
    {
        protected QueryHandlerBase()
        {
        }

        protected QueryHandlerBase(IValidator<TQuery> validator)
            : this()
        {
            this.Validator = validator;
        }

        protected virtual IValidator<TQuery> Validator { get; set; }

        public virtual async Task<IQueryResponse> Handle(TQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (this.Validator != null)
                {
                    this.ValidationResult = await this.Validator.ValidateAsync(query, cancellationToken);
                }

                if (this.ValidationResult == null || this.ValidationResult.IsValid)
                {
                    return await this.HandleAsync(query);
                }

                DomainException ex = this.HandleDomainException();
                return this.QueryResponseFail<TQueryResponse>().SetException<TQueryResponse>(ex);
            }
            catch (Exception ex)
            {
                return this.QueryResponseFail<TQueryResponse>().SetException<TQueryResponse>(ex);
            }
        }

        protected abstract Task<TQueryResponse> HandleAsync(TQuery query);
    }
}