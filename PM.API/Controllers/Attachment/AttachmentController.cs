using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Services.Shared;
using PM.InfrastructureModule.Domain.Shared;
using PM.InfrastructureModule.Entity.Shared;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Unity;

namespace PM.API.Controllers.Attachment
{
    [Route("api/v1/attachment")]
    [ApiController]
    [Authorize]
    public class AttachmentController : Controller
    {
        [UsedImplicitly] private readonly IAttachmentDomain _attachmentDomain;

        public AttachmentController()
        {
            _attachmentDomain = Bootstraper.Init().Resolve<AttachmentDomain>();
        }

        [HttpGet]
        [Route("{folder}/{objectGuid}")]
        [ProducesResponseType(typeof(List<AttachmentEntity>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Attachment"}, Summary = "Get list of attachments")]
        public async Task<ActionResult> SelectList(string folder, string objectGuid)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(objectGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var result = await _attachmentDomain.AttachmentGet(folder, objectGuid, null);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{folder}/{objectGuid}/{attachmentGuid}")]
        [ProducesResponseType(typeof(AttachmentEntity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Attachment"}, Summary = "Get attachment")]
        public async Task<ActionResult> Select(string folder, string objectGuid, string attachmentGuid)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(objectGuid) ||
                string.IsNullOrEmpty(attachmentGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var result = await _attachmentDomain.AttachmentGet(folder, objectGuid, attachmentGuid);
                var attachmentEntities = result.ToList();
                if (attachmentEntities.Any())
                {
                    return new OkObjectResult(attachmentEntities.FirstOrDefault());
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("{folder}/{object_guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Attachment"}, Summary = "Attach file to object")]
        public async Task<ActionResult> UploadFile(string folder, string object_guid)
        {
            try
            {
                var file = Request.Form.Files[0];

                if (string.IsNullOrEmpty(object_guid) || file.Length == 0)
                {
                    return new BadRequestResult();
                }

                var user = HttpContext.User;
                var userGuid = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                using (var fileContent = new MemoryStream())
                {
                    await file.CopyToAsync(fileContent);
                    await _attachmentDomain.AttachmentUpd(fileName, fileContent, folder, object_guid, serviceGroupGuid,
                        userGuid);
                }

                return new StatusCodeResult(200);
            }
            catch (System.Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{folder}/{objectGuid}/{attachmentGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Attachment"}, Summary = "Del attachment")]
        public async Task<ActionResult> Delete(string folder, string objectGuid, string attachmentGuid)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(objectGuid) ||
                string.IsNullOrEmpty(attachmentGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                await _attachmentDomain.AttachmentDel(attachmentGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}