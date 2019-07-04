using System;
using System.Text.RegularExpressions;

namespace PM.InfrastructureModule.Helpers.Data
{
    /// <summary>
    /// Validate input data
    /// </summary>
    public class Rg
    {
        public static bool IsCheck(string input, string regex)
        {
            if(string.IsNullOrEmpty(input))
                input = String.Empty;
            return Regex.IsMatch(input, $@"{regex}");
        }
    }
}