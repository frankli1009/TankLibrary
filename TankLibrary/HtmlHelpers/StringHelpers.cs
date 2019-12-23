using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TankLibrary.HtmlHelpers
{
    public static class StringHelpers
    {
        public static string AdjustString(this HtmlHelper html,
            string src, int stringLen, string suffix = "...")
        {
            if (src.Length <= stringLen) return src;

            string result = src.Substring(0, stringLen - suffix.Length).TrimEnd() + suffix;
            return result;
        }
    }
}