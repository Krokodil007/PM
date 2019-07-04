using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Entity.Log;
using PM.InfrastructureModule.Repository.StaticQuery.Log;
using Swashbuckle.AspNetCore.Annotations;

namespace PM.API.Controllers.Utils.Log
{
    [Route("api/v1/log")]
    [ApiController]
    [Authorize]
    public class LogController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(LogEntity), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Write log entry")]
        public async Task<ActionResult> WriteLog([FromBody] LogEntity item)
        {
            try
            {
                await LogInfo.LogEventIns(item.event_type_id, item.event_desc);
                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}