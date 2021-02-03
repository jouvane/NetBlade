using System.Text.RegularExpressions;

namespace NetBlade.CrossCutting.Helpers
{
    public static class StringHelper
    {
        public static string OnlyNumbers(this string value)
        {
            return string.Join(string.Empty, Regex.Split(value ?? string.Empty, @"[^\d]"));
        }

        public static string RemoveSpecialCharacter(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return Regex.Replace(value, "[^a-zA-Z0-9]", string.Empty);
        }
    }
}
