using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace NetBlade.Core.Exceptions
{
    public static class ValidationResultExtension
    {
        public static DomainException ParserValidatorResult<T>(this ValidationResult result)
        {
            if (!result.IsValid && result.Errors.Any())
            {
                ValidationResultDomainException domainException = new ValidationResultDomainException();
                domainException.AddValidationsErrors(result.Errors);

                return domainException;
            }

            return null;
        }

        public static DomainException Validate<T>(this AbstractValidator<T> validator, T entity)
        {
            if (entity == null || validator == null)
            {
                return null;
            }

            ValidationResult result = validator.Validate(entity);
            return result.ParserValidatorResult<T>();
        }
    }
}
