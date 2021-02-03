using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace NetBlade.CrossCutting.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(Enum item)
        {
            MemberInfo memberInfo = (
                from member in item.GetType().GetMember(item.ToString())
                where member.MemberType == MemberTypes.Field
                select member).FirstOrDefault();

            DisplayAttribute displayAttribute = AttributeHelper.ExtractAttribute<DisplayAttribute>(memberInfo);
            if (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.Name))
            {
                if (displayAttribute.ResourceType != null)
                {
                    ResourceManager resourceManager = new ResourceManager(displayAttribute.ResourceType);
                    return resourceManager.GetString(displayAttribute.Name);
                }

                return displayAttribute.Name;
            }

            return item.ToString();
        }

        public static T GetItemByValue<T>(Type enumType, string value)
        {
            enumType = TypeHelper.GetUnderlyingType(enumType);
            Array values = Enum.GetValues(enumType);
            foreach (object item in values)
            {
                if (((int)item).ToString() == value)
                {
                    return (T)item;
                }
            }

            return default;
        }

        public static List<Tuple<string, string>> ListAllItemEnum<TEnum>()
            where TEnum : struct
        {
            return EnumHelpers.ListAllItemEnum(typeof(TEnum));
        }

        public static List<Tuple<string, string>> ListAllItemEnum(Type enumType)
        {
            enumType = TypeHelper.GetUnderlyingType(enumType);
            return
            (
                from w in enumType.GetFields()
                where w.GetCustomAttribute<DisplayAttribute>() != null
                select w
                into o
                orderby o.GetCustomAttribute<DisplayAttribute>().GetOrder(), EnumHelpers.GetDescription((Enum)o.GetValue(enumType))
                select o
                into s
                select new Tuple<string, string>(Convert.ChangeType(s.GetValue(enumType), ((Enum)s.GetValue(enumType)).GetTypeCode()).ToString(), EnumHelpers.GetDescription((Enum)s.GetValue(enumType)))
            ).ToList();
        }
    }
}
