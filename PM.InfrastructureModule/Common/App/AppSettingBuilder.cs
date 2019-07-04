using System.IO;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace PM.InfrastructureModule.Common.App
{
    /// <summary>
    /// Работа с конфигурационным файлом
    /// </summary>
    [UsedImplicitly]
    public class AppSettingBuilder
    {
        /// <summary>
        /// Сборка конфигурационного файла
        /// </summary>
        public static IConfigurationRoot GetAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}
