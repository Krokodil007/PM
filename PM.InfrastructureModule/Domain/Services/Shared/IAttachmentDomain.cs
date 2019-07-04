using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PM.InfrastructureModule.Entity.Shared;

namespace PM.InfrastructureModule.Domain.Services.Shared
{
    /// <summary>
    /// Attachments
    /// </summary>
    public interface IAttachmentDomain
    {
        /// <summary>
        /// Attachment get
        /// </summary>
        Task<IEnumerable<AttachmentEntity>> AttachmentGet(string container, string objectGuid,
            string attachmentGuid);

        /// <summary>
        /// Attachment upload
        /// </summary>
        Task AttachmentUpd(string fileName, MemoryStream fileContent, string containerName,
            string objectGuid, string serviceGroupGid, string userGuid);

        /// <summary>
        /// Attachment del
        /// </summary>
        Task AttachmentDel(string attachmentGuid);
    }
}