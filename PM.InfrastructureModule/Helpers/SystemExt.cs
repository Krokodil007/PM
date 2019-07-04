using System;
using System.Text;

namespace PM.InfrastructureModule.Helpers
{
    /// <summary>
    /// Infra Sys Helpers
    /// </summary>
    public class SystemExt
    {
        /// <summary>
        /// GetBase 64 encodee String
        /// </summary>
        public static string GetStringToBase64(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }
}