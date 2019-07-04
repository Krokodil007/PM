using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Domain.Services.Shared;
using PM.InfrastructureModule.Entity.Shared;
using PM.InfrastructureModule.Repository.StaticQuery.Attachment;

namespace PM.InfrastructureModule.Domain.Shared
{
    /// <summary>
    /// Файлы
    /// </summary>
    public class AttachmentDomain : IAttachmentDomain
    {

        /// <summary>
        /// Attachment get
        /// </summary>
        public async Task<IEnumerable<AttachmentEntity>> AttachmentGet(string container, string objectGuid,
            string attachmentGuid)
        {
            var result = await AttachmentInfo.AttachmentGet(container, objectGuid, attachmentGuid);
            return result;
        }

        /// <summary>
        /// Attachment upload
        /// </summary>
        public async Task AttachmentUpd(string fileName, MemoryStream fileContent, string containerName,
            string objectGuid, string serviceGroupGid, string userGuid)
        {
            fileContent.Position = 0;
            var uploadedFile = new AttachmentEntity
            {
                container = containerName,
                name = fileName,
                content = fileContent,
                object_guid = objectGuid,
                service_group_guid = serviceGroupGid,
                user_guid = userGuid
            };

            var resultFile = await AzureStorage.UploadFileAzureStorage(uploadedFile);
            await AttachmentInfo.AttachmentUpd(resultFile);
        }

        /// <summary>
        /// Attachment del
        /// </summary>
        public async Task AttachmentDel(string attachmentGuid)
        {
            await AttachmentInfo.AttachmentUpdDel(attachmentGuid);
        }
    }
}