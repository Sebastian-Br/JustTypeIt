using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JustTypeIt
{
    internal class GenericString
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
    }
}