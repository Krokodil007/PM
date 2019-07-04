using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Entity.Identity;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace PM.API.Controllers.Identity.Auth
{
    [Route("api/v1/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : Controller
    {
        [HttpGet]
        [Route("context")]
        [ProducesResponseType(typeof(AuthContext), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Get AuthContext", Description = "Get Authorization context object for current user")]
        public async Task<ActionResult> GetAuthContext()
        {
            try
            {
                var user =  HttpContext.User;
                var userProfile = await UserIdentitInfo.GetUserProfile(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var authContext = new AuthContext
                {
                    Name = userProfile?.user_fullname,
                    Email = user.FindFirstValue(ClaimTypes.Email),
                    Guid = user.FindFirstValue(ClaimTypes.NameIdentifier),
                    service_group_guid = await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Claims = user.Claims.Where(c => c.Type.Contains("pm_"))
                        .Select(c => new SimpleClaim {Type = c.Type, Value = c.Value}).ToList(),
                    Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.ToString()).ToList()
                };
                return new OkObjectResult(authContext);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}