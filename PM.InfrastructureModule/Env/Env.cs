using System.Collections.Generic;
using System.Linq;

namespace PM.InfrastructureModule.Env
{
    /// <summary>
    /// Environment Variable
    /// </summary>
    public static class Env
    {
        /// <summary>
        ///  Local Security Token Server
        /// </summary>
        public static string StsLocalServer { get; } = "http://localhost:4242";

        /// <summary>
        ///  Security Token Server
        /// </summary>
        public static string StsSecondaryServer { get; } = "https://auth-sts.azurewebsites.net/";

        /// <summary>
        ///  Security Token Server
        /// </summary>
        public static string StsPrimaryServer { get; } = "https://auth2.host/";

        /// <summary>
        /// Application ValidAudience
        /// </summary>
        public static string AppAuthorize { get; } = @"pm_web_api";

        /// <summary>
        /// Localhost Dev APP
        /// </summary>
        public static string LocalClientApp { get; } = @"http://localhost:4300";

        /// <summary>
        /// Developer Azure APP
        /// </summary>
        public static string DevClientApp { get; } = @"https://pixmake.azurewebsites.net";


        /// <summary>
        /// List redirect hosts
        /// </summary>
        public static string GetListRedirectHosts(string hostKey)
        {
            if (string.IsNullOrEmpty(hostKey))
                return string.Empty;

            var list = new List<Host>
            {
                new Host {HostKey = "Un9Nfh4u", HostValue = "http://localhost:4300/login"},
                new Host {HostKey = "CkzQ1hsb", HostValue = "https://pixmake.azurewebsites.net/login"}
            };
            return list.FirstOrDefault(c => c.HostKey.Contains(hostKey))?.HostValue;
        } 
    }

    public class Host
    {
        public string HostKey { get; set; }
        public string HostValue { get; set; }
    }
}