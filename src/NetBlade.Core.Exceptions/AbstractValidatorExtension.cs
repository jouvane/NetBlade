using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace NetBlade.Core.Exceptions
{
    public static class AbstractValidatorExtension
    {
        public static void ValidateAndThrowException<T>(this AbstractValidator<T> validator, T value)
        {
            DomainException exception = validator.Validate<T>(value);
            if (exception != null)
            {
                throw exception;
            }
        }

        public static async Task ValidateAndThrowExceptionAsync<T>(this AbstractValidator<T> validator, T value)
        {
            DomainException exception = await validator.ValidateAsync<T>(value);
            if (exception != null)
            {
                throw exception;
            }
        }

        public static async Task<DomainException> ValidateAsync<T>(this AbstractValidator<T> validator, T entity)
        {
            if (entity == null || validator == null)
            {
                return null;
            }

            ValidationResult result = await validator.ValidateAsync(entity);
            return result.ParserValidatorResult<T>();
        }
    }
}
