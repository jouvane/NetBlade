using FluentValidation;
using NetBlade.Core.Domain;
using NetBlade.Core.Events;
using NetBlade.Core.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBlade.Core.Services
{
    public static class ServiceBaseExtension
    {
        public static Task RaiseEvent(this ServiceBase serviceBase, Event @event)
        {
            return serviceBase.RaiseEvents(new List<Event> { @event });
        }

        public static Task RaiseEvents(this ServiceBase serviceBase, Entity entity)
        {
            if (entity.HasEvents())
            {
                return serviceBase.RaiseEvents(entity.GetEvents());
            }

            return Task.CompletedTask;
        }

        public static DomainException Validate<T>(this ServiceBase serviceBase, T entity, AbstractValidator<T> validator)
        {
            return validator.Validate<T>(entity);
        }

        public static void ValidateAndThrowException<T>(this ServiceBase serviceBase, T value, AbstractValidator<T> validator)
        {
            validator.ValidateAndThrowException(value);
        }

        public static Task ValidateAndThrowExceptionAsync<T>(this ServiceBase serviceBase, T value, AbstractValidator<T> validator)
        {
            return validator.ValidateAndThrowExceptionAsync(value);
        }

        public static Task<DomainException> ValidateAsync<T>(this ServiceBase serviceBase, T entity, AbstractValidator<T> validator)
        {
            return validator.ValidateAsync<T>(entity);
        }
    }
}
