using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetBlade.CrossCutting.Helpers
{
    public static class OrderedQueryableHelper
    {
        public static IOrderedQueryable<T> OrderByPropertyName<T>(IEnumerable<T> query, string propertyName, string sortDirection)
        {
            return OrderedQueryableHelper.OrderByPropertyName(query, propertyName, "ASC".Equals((sortDirection ?? "ASC").ToUpper()));
        }

        public static IOrderedQueryable<T> OrderByPropertyName<T>(IEnumerable<T> query, string propertyName, bool orderAscending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            Type type = typeof(T);

            ParameterExpression parameterExpression = Expression.Parameter(type, "x");
            Expression expression = parameterExpression;

            string[] array = propertyName.Split(new[]
            {
                '.'
            });

            for (int i = 0; i < array.Length; i++)
            {
                string propertyOrFieldName = array[i];
                expression = Expression.PropertyOrField(expression, propertyOrFieldName);
                type = expression.Type;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);

            LambdaExpression lambdaExpression = Expression.Lambda(delegateType, expression, parameterExpression);
            string methodName = orderAscending ? "OrderBy" : "OrderByDescending";

            object? result = typeof(Queryable).GetMethods()
               .First(
                    method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2)
               .MakeGenericMethod(typeof(T), type)
               .Invoke(null, new object[] { query.AsQueryable(), lambdaExpression });

            return (IOrderedQueryable<T>)result;
        }
    }
}
