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
    public class ContactBankAccountController : Controller
    {
        [UsedImplicitly] private readonly IContactDomain _contactDomain;

        public ContactBankAccountController()
        {
            _contactDomain = Bootstraper.Init().Resolve<ContactDomain>();
        }

        [HttpGet]
        [Route("{contactGuid}/bank_account")]
        [ProducesResponseType(typeof(List<ContactBankAccountDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactBankAccount"}, Summary = "Get list of contact bank accounts")]
        public async Task<ActionResult> SelectList(string contactGuid)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _contactDomain.ContactBankAccountGet(null, contactGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("{contactGuid}/bank_account")]
        [ProducesResponseType(typeof(List<ContactBankAccountDto>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactBankAccount"}, Summary = "Create new/update contact bank account")]
        public async Task<ActionResult> Create([FromBody] List<ContactBankAccountDto> itemList)
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
                var result = await _contactDomain.ContactBankAccountUpd(itemList, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("bank_account/{contactBankAccountGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"ContactBankAccount"}, Summary = "Delete contact bank account")]
        public async Task<ActionResult> Delete(string contactBankAccountGuid)
        {
            if (string.IsNullOrEmpty(contactBankAccountGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await ContactInfo.ContactBankAccountDel(contactBankAccountGuid, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}