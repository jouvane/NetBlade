using NetBlade.Core.Exceptions;
using System.Linq;
using System.Reflection;

namespace NetBlade.Core.Commands
{
    public static class CommandResponseExtension
    {
        public static T CommandResponse<T>(this CommandHandlerBase handlerBase, bool sucess)
        {
            return (T)typeof(T)
               .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
               .First(f => f.GetParameters().Any(a => a.ParameterType == typeof(bool)))
               .Invoke(new object[] { sucess });
        }

        public static CommandResponse CommandResponseFail(this CommandHandlerBase handlerBase)
        {
            return handlerBase.CommandResponse<CommandResponse>(false);
        }

        public static T CommandResponseFail<T>(this CommandHandlerBase handlerBase)
            where T : ICommandResponse
        {
            return handlerBase.CommandResponse<T>(false);
        }

        public static CommandResponse CommandResponseOk(this CommandHandlerBase handlerBase)
        {
            return handlerBase.CommandResponse<CommandResponse>(true);
        }

        public static T CommandResponseOk<T>(this CommandHandlerBase handlerBase)
            where T : ICommandResponse
        {
            return handlerBase.CommandResponse<T>(true);
        }

        public static T CommandResponseOk<T, TData>(this CommandHandlerBase handlerBase, TData data)
            where T : CommandResponse<TData>
        {
            T result = handlerBase.CommandResponse<T>(true);
            result.Data = data;

            return result;
        }

        public static CommandResponse<TData> CommandResponseOk<TData>(this CommandHandlerBase handlerBase, TData data)
        {
            CommandResponse<TData> result = handlerBase.CommandResponse<CommandResponse<TData>>(true);
            result.Data = data;

            return result;
        }

        public static DomainException HandleDomainException(this CommandHandlerBase handlerBase)
        {
            ValidationResultDomainException ex = new ValidationResultDomainException();
            ex.AddValidationsErrors(handlerBase.ValidationResult?.Errors);

            return ex;
        }
    }
}
