using FluentValidation.Results;
using NetBlade.Core.Exceptions.Test.Model;
using System;
using Xunit;

namespace NetBlade.Core.Exceptions.Test
{
    public class DomainExceptionTest
    {
        [Fact]
        public void DomainExceptionAddValidationsErrorsNotNullTest()
        {
            ValidationResultDomainException ex = new ValidationResultDomainException();
            ex.AddValidationsErrors(new[] { new ValidationFailure("msg", "msg") });
            Assert.NotNull(ex.Validations);
        }

        [Fact]
        public void DomainExceptionMessageNullTest()
        {
            Assert.NotNull(new DomainException());
        }

        [Fact]
        public void DomainExceptionmessageTest()
        {
            Assert.Equal("messageRR", new DomainException("messageRR").Message);
        }

        [Fact]
        public void DomainExceptionNotRollbackTransactionTest()
        {
            Exception ex = new DomainException("messageRR");
            Assert.False(ex is INotRollbackTransaction);
        }

        [Fact]
        public void DomainExceptionRollbackTransactionTest()
        {
            Exception ex = new AlertaDomainException("messageRR");

            Assert.True(ex is INotRollbackTransaction);
            Assert.Equal("messageRR", ex.Message);
        }

        [Fact]
        public void DomainExceptionValidationsNotNullTest()
        {
            ValidationResultDomainException ex = new ValidationResultDomainException();
            ex.AddValidationError(new ValidationFailure("msg", "msg"));
            Assert.NotNull(ex.Validations);
        }
    }
}
