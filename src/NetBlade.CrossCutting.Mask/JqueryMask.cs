using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetBlade.CrossCutting.Mask
{
    public class JqueryMask : IMask
    {
        private static readonly char[] valuesAcceptedReplace = { '0', '9', '*', 'A', 'S' };

        public JqueryMask(string[] maskTemplete)
        {
            this.MaskTemplete = maskTemplete.OrderBy(o => o.Length).ToArray();
        }

        public string[] MaskTemplete { get; }

        public string CleanValue(string value)
        {
            value ??= string.Empty;
            Regex regExp = new Regex("[0-9]|[S]|[A]|[*]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            foreach (string mask in this.MaskTemplete)
            {
                string marks = regExp.Replace(mask, string.Empty);
                if (value.Length <= mask.Length)
                {
                    foreach (char mark in marks.Distinct())
                    {
                        value = value.Replace(mark.ToString(), string.Empty);
                    }

                    return value;
                }
            }

            return value;
        }

        public string Format(string value)
        {
            value ??= string.Empty;
            string maskedValue = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                foreach (string mask in this.MaskTemplete)
                {
                    string maskClean = this.CleanValue(mask);
                    string valueClean = this.CleanValue(value);
                    valueClean = valueClean.PadLeft(maskClean.Length, '0');

                    if (maskClean.Length >= valueClean.Length)
                    {
                        int indexText = 0;
                        for (int i = 0; i < mask.Length; i++)
                        {
                            if (valueClean.Length > indexText)
                            {
                                if (JqueryMask.valuesAcceptedReplace.Contains(mask[i]))
                                {
                                    maskedValue += valueClean[indexText++];
                                }
                                else
                                {
                                    maskedValue += mask[i];
                                }
                            }
                            else if (!JqueryMask.valuesAcceptedReplace.Contains(mask[i]))
                            {
                                maskedValue += mask[i];
                            }
                        }

                        return maskedValue;
                    }
                }
            }

            return value;
        }
    }
}
