using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PM.InfrastructureModule.Common.App;

namespace PM.InfrastructureModule.DataCore
{
    /// <summary>
    /// DataStore PixMake
    /// </summary>
    [UsedImplicitly]
    public class LogMySqlRepository
    {
        private static readonly IConfigurationRoot Config = AppSettingBuilder.GetAppSettings();

        /// <summary>
        /// Database String Connection
        /// </summary>
        [CanBeNull] private static readonly string ConnectionString = Config.GetConnectionString("log_mysql");

        /// <summary>
        /// Return Sql Server Connection
        /// </summary>
        public static async Task<MySqlConnection> GetConnection()
        {
            var sqlConnection = new MySqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}