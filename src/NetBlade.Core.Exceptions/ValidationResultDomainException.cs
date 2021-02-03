using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetBlade.Core.Exceptions
{
    public class ValidationResultDomainException : DomainException
    {
        public override string Message
        {
            get => string.Join(Environment.NewLine, this.Validations.Select(s => string.Join(Environment.NewLine, s.ErrorMessage)));
        }

        public IList<ValidationFailure> Validations { get; set; }

        public void AddValidationError(ValidationFailure result)
        {
            this.Validations = this.Validations ?? new List<ValidationFailure>();
            this.Validations.Add(result);
        }

        public void AddValidationsErrors(IList<ValidationFailure> validations)
        {
            this.Validations = validations;
        }
    }
}
