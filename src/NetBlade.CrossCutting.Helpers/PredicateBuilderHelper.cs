using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetBlade.CrossCutting.Helpers
{
    public static class PredicateBuilderHelper
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, bool apply, Expression<Func<T, bool>> b)
        {
            return apply ? PredicateBuilderHelper.And(a, b) : a;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression parameter = SubstExpressionVisitor.GetParameter(a);
            Expression body = Expression.AndAlso(a.Body, SubstExpressionVisitor.GetVisit(a, b, parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, bool apply, Expression<Func<T, bool>> b)
        {
            return apply ? PredicateBuilderHelper.Or(a, b) : a;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression parameter = SubstExpressionVisitor.GetParameter(a);
            Expression body = Expression.OrElse(a.Body, SubstExpressionVisitor.GetVisit(a, b, parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private class SubstExpressionVisitor : ExpressionVisitor
        {
            private readonly Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

            public static ParameterExpression GetParameter<T1, T2>(Expression<Func<T1, T2>> a)
            {
                ParameterExpression p = a.Parameters[0];
                return p;
            }

            public static Expression GetVisit<T1, T2>(Expression<Func<T1, T2>> a, Expression<Func<T1, T2>> b, ParameterExpression parameter = null)
            {
                SubstExpressionVisitor visitor = new SubstExpressionVisitor();
                visitor.subst[b.Parameters[0]] = parameter ?? SubstExpressionVisitor.GetParameter(a);

                return visitor.Visit(b.Body);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (this.subst.TryGetValue(node, out Expression newValue))
                {
                    return newValue;
                }

                return node;
            }
        }
    }
}
