using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.Mail;
using PM.InfrastructureModule.Entity.Mail;
using Swashbuckle.AspNetCore.Annotations;

namespace PM.API.Controllers.Utils.Mailer
{
    [Route("api/v1/mail")]
    [ApiController]
    [Authorize]

    public class MailManagerController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(List<MailerManagerMessage>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Send mail messages", Description = "Send mail messages via MailerManager")]
        public async Task<ActionResult> Send([FromBody] List<MailerManagerMessage> messageList)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                await MailerManager.MailSenderAsync(messageList);
                return new OkObjectResult(messageList);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}