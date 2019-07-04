using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Contact;
using PM.InfrastructureModule.Domain.Services.Contact;
using PM.InfrastructureModule.Domain.Services.Shared;
using PM.InfrastructureModule.Domain.Shared;
using PM.InfrastructureModule.Dto.Contact;
using PM.InfrastructureModule.Repository.StaticQuery.Contact;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Unity;

namespace PM.API.Controllers.Contact
{
    [Route("api/v1/contact")]
    [ApiController]
    [Authorize]
    public class ContactController : Controller
    {
        [UsedImplicitly] private readonly IContactDomain _contactDomain;
        [UsedImplicitly] private readonly IAttachmentDomain _attachmentDomain;

        public ContactController()
        {
            _contactDomain = Bootstraper.Init().Resolve<ContactDomain>();
            _attachmentDomain = Bootstraper.Init().Resolve<AttachmentDomain>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ContactDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Contact"}, Summary = "Get list of contacts")]
        public async Task<ActionResult> SelectList()
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _contactDomain.ContactGet(null, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{contactGuid}")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Contact"}, Summary = "Get contact")]
        public async Task<ActionResult> Select(string contactGuid)
        {
            if (string.IsNullOrEmpty(contactGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _contactDomain.ContactGet(contactGuid, serviceGroupGuid);
                var contactEntities = result.ToList();
                if (contactEntities.Any())
                {
                    return new OkObjectResult(contactEntities.FirstOrDefault());
                }
                else
                {
                    return new OkObjectResult(new ContactDto());
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContactDto), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Contact"}, Summary = "Create new contact")]
        public async Task<ActionResult> Create([FromBody] ContactDto item)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var user = HttpContext.User;
                var userGuid = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                
                var result = await _contactDomain.ContactUpd(item, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);

            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [Route("{contactGuid}/approved/{approved}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Contact"}, Summary = "Update contact approved status")]
        public async Task<ActionResult> UpdateApproved(string contactGuid, bool approved)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await ContactInfo.ContactApprovedUpd(contactGuid, approved, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{contactGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Contact"}, Summary = "Delete contact")]
        public async Task<ActionResult> Delete(string contactGuid)
        {
            if (string.IsNullOrEmpty(contactGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await ContactInfo.ContactDel(contactGuid, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}