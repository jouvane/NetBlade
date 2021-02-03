using System;
using System.Linq.Expressions;

namespace NetBlade.CrossCutting.Helpers
{
    public static class LambdaExpressionHelper
    {
        public static Expression GetExpressionByPropertyName(ParameterExpression parameterExpression, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            Expression expression = parameterExpression;
            string[] array = propertyName.Split(new[] { '.' });

            for (int i = 0; i < array.Length; i++)
            {
                string propertyOrFieldName = array[i];
                expression = Expression.PropertyOrField(expression, propertyOrFieldName);
            }

            return expression;
        }

        public static Func<TSource, TResult> GetLambdaExpressionByPropertyName<TSource, TResult>(string propertyName)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource), "x");
            Expression expression = LambdaExpressionHelper.GetExpressionByPropertyName(parameterExpression, propertyName);

            return Expression.Lambda<Func<TSource, TResult>>(expression, parameterExpression).Compile();
        }
    }
}
