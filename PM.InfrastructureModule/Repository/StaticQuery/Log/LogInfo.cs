using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;

namespace PM.InfrastructureModule.Repository.StaticQuery.Log
{
    /// <summary>
    /// Log DataStore Info
    /// </summary>
    [UsedImplicitly]
    public class LogInfo
    {
        /// <summary>
        /// Log Ins
        /// </summary>
        public static async Task LogEventIns(int eventTypeId, string eventDescripton)
        {
            try
            {
                var logRepository = await LogMySqlRepository.GetConnection();
                await logRepository.ExecuteAsync("log_event_ins", new
                    {
                        event_type_id_in = eventTypeId,
                        event_desc_in = eventDescripton
                    },
                    commandType: CommandType.StoredProcedure);
                logRepository.Close();
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
            }
        }
    }
}