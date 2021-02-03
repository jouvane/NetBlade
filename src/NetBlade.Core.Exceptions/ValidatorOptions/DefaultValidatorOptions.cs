using FluentValidation;
using FluentValidation.Resources;
using System;

namespace NetBlade.Core.Exceptions.ValidatorOptions
{
    public static class NetBladeDefaultValidatorOptions
    {
        public static IRuleBuilderOptions<T, TProperty> WithMessageDomainException<T, TDomainException, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Func<T, TDomainException> messageProvider)
            where TDomainException : DomainException
        {
            return rule
               .Configure(config =>
                {
                    config.CurrentValidator.Options.ErrorCodeSource = new StaticStringSource(messageProvider.Method.ReturnType.FullName);
                    config.CurrentValidator.Options.ErrorMessageSource = new LazyStringSource(ctx => messageProvider((T)ctx?.InstanceToValidate).Message);
                });
        }
    }
}
