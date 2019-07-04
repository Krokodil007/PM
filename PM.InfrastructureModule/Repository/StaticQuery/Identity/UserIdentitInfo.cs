using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Identity;

namespace PM.InfrastructureModule.Repository.StaticQuery.Identity
{
    /// <summary>
    /// User DataStore Info
    /// </summary>
    [UsedImplicitly]
    public class UserIdentitInfo
    {
        /// <summary>
        /// User Identity Info
        /// </summary>
        public static async Task<UserProfile> GetUserProfile(string userGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<UserProfile>("auth_user_profile_get",
                    new { user_guid_in = userGuid },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
            }

            return new UserProfile();
        }

        /// <summary>
        /// User Identity Info
        /// </summary>
        public static async Task<string> GetUserServiceGroup(string userGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<string>("auth_service_group_account_get",
                    new { user_guid_in = userGuid },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
            }

            return null;
        }
    }
}