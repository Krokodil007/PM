using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.DataCore;
using PM.InfrastructureModule.Entity.Shared;

namespace PM.InfrastructureModule.Repository.StaticQuery.Attachment
{
    /// <summary>
    /// AttachmentInfo
    /// </summary>
    [UsedImplicitly]
    public class AttachmentInfo
    {
        /// <summary>
        /// Attachment Get
        /// </summary>
        public static async Task<IEnumerable<AttachmentEntity>> AttachmentGet(string container, string objectGuid,
            string attachmentGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryAsync<AttachmentEntity>("pm_attachment_get",
                    new
                    {
                        container_in = container,
                        object_guid_in = objectGuid,
                        attachment_guid_in = attachmentGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// Attachment Upd
        /// </summary>
        public static async Task<AttachmentEntity> AttachmentUpd(AttachmentEntity item)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                var result = await pixMakeRepository.QueryFirstOrDefaultAsync<AttachmentEntity>(
                    "pm_attachment_upd", new
                    {
                        service_group_guid_in = item.service_group_guid,
                        user_guid_in = item.user_guid,
                        container_in = item.container,
                        object_guid_in = item.object_guid,
                        name_in = item.name,
                        uri_in = item.uri
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
                return result;
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
                return null;
            }
        }

        /// <summary>
        /// Attachment Del
        /// </summary>
        public static async Task AttachmentUpdDel(string attachmentGuid)
        {
            try
            {
                var pixMakeRepository = await PixMakeMySqlRepository.GetConnection();
                await pixMakeRepository.ExecuteAsync("pm_attachment_del", new
                    {
                        attachment_guid_in = attachmentGuid
                    },
                    commandType: CommandType.StoredProcedure);
                pixMakeRepository.Close();
            }
            catch (Exception ex)
            {
                await SendError.SendErrorAsync(ex, customMessage: "MySql");
            }
        }
    }
}