using NetBlade.Core.Exceptions;
using System.Linq;
using System.Reflection;

namespace NetBlade.Core.Querys
{
    public static class QueryResponseExtension
    {
        public static DomainException HandleDomainException(this QueryHandlerBase handlerBase)
        {
            ValidationResultDomainException ex = new ValidationResultDomainException();
            ex.AddValidationsErrors(handlerBase.ValidationResult?.Errors);

            return ex;
        }

        public static T QueryResponse<T>(this QueryHandlerBase handlerBase, bool sucess)
        {
            return (T)typeof(T)
               .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
               .First(f => f.GetParameters().Any(a => a.ParameterType == typeof(bool)))
               .Invoke(new object[] { sucess });
        }

        public static T QueryResponseFail<T>(this QueryHandlerBase handlerBase)
            where T : IQueryResponse
        {
            return handlerBase.QueryResponse<T>(false);
        }

        public static T QueryResponseOk<T, TData>(this QueryHandlerBase handlerBase, TData data)
            where T : QueryResponse<TData>
        {
            T result = handlerBase.QueryResponse<T>(true);
            result.Data = data;

            return result;
        }

        public static QueryResponse<TData> QueryResponseOk<TData>(this QueryHandlerBase handlerBase, TData data)
        {
            QueryResponse<TData> result = handlerBase.QueryResponse<QueryResponse<TData>>(true);
            result.Data = data;

            return result;
        }

        public static T QueryResponseOk<T>(this QueryHandlerBase handlerBase)
            where T : IQueryResponse
        {
            return handlerBase.QueryResponse<T>(true);
        }
    }
}
