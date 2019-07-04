using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Contact;
using PM.InfrastructureModule.Domain.Services.Contact;
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
    public class ContactPersonController : Controller
    {
        [UsedImplicitly] private readonly IContactDomain _contactDomain;

        public ContactPersonController()
        {
            _contactDomain = Bootstraper.Init().Resolve<ContactDomain>();
        }

        [HttpGet]
        [Route("{contactGuid}/person")]
        [ProducesResponseType(typeof(List<ContactPersonDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactPerson"}, Summary = "Get list of contact persons")]
        public async Task<ActionResult> SelectList(string contactGuid)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _contactDomain.ContactPersonGet(null, contactGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("{contactGuid}/person")]
        [ProducesResponseType(typeof(List<ContactPersonDto>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactPerson"}, Summary = "Create new/update contact person")]
        public async Task<ActionResult> Create([FromBody] List<ContactPersonDto> itemList)
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
                var result = await _contactDomain.ContactPersonUpd(itemList, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("/person/{contactPersonGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactPerson"}, Summary = "Delete contact person")]
        public async Task<ActionResult> Delete(string contactPersonGuid)
        {
            if (string.IsNullOrEmpty(contactPersonGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await ContactInfo.ContactPersonDel(contactPersonGuid, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}