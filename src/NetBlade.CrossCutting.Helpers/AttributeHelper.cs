using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetBlade.CrossCutting.Helpers
{
    public static class AttributeHelper
    {
        public static Attr ExtractAttribute<Attr, TModel, TValue>(Expression<Func<TModel, TValue>> expression)
            where Attr : Attribute
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            return AttributeHelper.ExtractAttribute<Attr>(memberExpression.Member);
        }

        public static Attr ExtractAttribute<Attr>(MemberInfo memberInfo)
            where Attr : Attribute
        {
            object[] attrs = memberInfo.GetCustomAttributes(typeof(Attr), true);
            Attr attr = null;

            if (attrs != null && attrs.Length == 1)
            {
                attr = attrs.Cast<Attr>().First();
            }

            return attr;
        }
    }
}
