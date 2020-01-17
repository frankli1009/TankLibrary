using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TankLibrary.HtmlHelpers
{
    public static class StringHelpers
    {
        public static string AdjustString(this HtmlHelper html, string src, int stringLen, string suffix = "...")
        {
            return src == null? src : src.AdjustString(stringLen, suffix);
        }

        public static string AdjustString(this string src, int stringLen, string suffix = "...")
        {
            if (src.Length <= stringLen) return src;

            string result = src.Substring(0, stringLen - suffix.Length).TrimEnd() + suffix;
            return result;
        }

        public static string AdjustEmailString(this string email, int stringLen, string suffix = "...")
        {
            if (email.Length <= stringLen) return email;

            int index = email.IndexOf('@');
            if (index > -1)
            {
                string emailName = email.Substring(0, index);
                return emailName.AdjustString(stringLen, suffix);
            }
            else return email.AdjustString(stringLen, suffix);
        }
    }
}