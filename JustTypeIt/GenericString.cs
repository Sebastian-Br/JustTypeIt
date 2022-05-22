using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JustTypeIt
{
    internal static class GenericString
    {
        // use Trim() to remove leading/trailing whitespaces.

        public static string RemoveSpacesAroundCommata(string input)
        {
            try
            {
                string output = Regex.Replace(input, " *, *", ",");
                return output;
            }
            catch (Exception ex)
            {
                return input;
            }
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}