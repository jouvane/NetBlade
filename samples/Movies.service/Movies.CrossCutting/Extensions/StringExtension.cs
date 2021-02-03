using System;
using System.Movieslections.Generic;
using System.Text;

namespace Movies.CrossCutting.Extensions
{
    public static class StringExtension
    {
        public static string DiacriticsClearString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                string withDiacritics = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
                string withoutDiacritics = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
                for (int i = 0; i < withDiacritics.Length; i++)
                {
                    str = str.Replace(withDiacritics[i].ToString(), withoutDiacritics[i].ToString());
                }

                return str;
            }
        }
    }
}
