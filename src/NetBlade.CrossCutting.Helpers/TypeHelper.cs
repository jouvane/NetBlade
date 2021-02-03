using System;
using System.Reflection;

namespace NetBlade.CrossCutting.Helpers
{
    public static class TypeHelper
    {
        public static bool CheckIsDerived<T, TTDerived>()
        {
            return TypeHelper.CheckIsDerived(typeof(T), typeof(TTDerived));
        }

        public static bool CheckIsDerived(Type type, Type derivedTypeCheck)
        {
            while (derivedTypeCheck != null && derivedTypeCheck != typeof(object))
            {
                Type right = derivedTypeCheck.IsGenericType ? derivedTypeCheck.GetGenericTypeDefinition() : derivedTypeCheck;
                if (type == right)
                {
                    return true;
                }

                derivedTypeCheck = derivedTypeCheck.BaseType;
            }

            return false;
        }

        public static ConstructorInfo GetCompatibleConstructor<T>(object[] args)
        {
            return TypeHelper.GetCompatibleConstructor(typeof(T), args);
        }

        public static ConstructorInfo GetCompatibleConstructor(Type type, object[] args)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors != null)
            {
                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();
                    if (parameters.Length != args.Length)
                    {
                        continue;
                    }

                    if (parameters.Length == 0 && args.Length == 0)
                    {
                        return constructor;
                    }

                    bool found = true;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterInfo parameter = parameters[i];
                        object arg = args[i];
                        if (arg.GetType().IsInstanceOfType(parameter.GetType()))
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        return constructor;
                    }
                }
            }

            return null;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
