using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Equipment;
using PM.InfrastructureModule.Domain.Services.Equipment;
using PM.InfrastructureModule.Dto.Equipment;
using PM.InfrastructureModule.Repository.StaticQuery.Equipment;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Unity;

namespace PM.API.Controllers.Equipment
{
    [Route("api/v1/equipment")]
    [ApiController]
    [Authorize]
    public class EquipmentController : Controller
    {
        [UsedImplicitly] private readonly IEquipmentDomain _equipmentDomain;

        public EquipmentController()
        {
            _equipmentDomain = Bootstraper.Init().Resolve<EquipmentDomain>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EquipmentDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Equipment"}, Summary = "Get list of equipment")]
        public async Task<ActionResult> SelectList()
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _equipmentDomain.EquipmentGet(null, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{equipmentGuid}")]
        [ProducesResponseType(typeof(EquipmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Equipment"}, Summary = "Get equipment")]
        public async Task<ActionResult> Select(string equipmentGuid)
        {
            if (string.IsNullOrEmpty(equipmentGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _equipmentDomain.EquipmentGet(equipmentGuid, serviceGroupGuid);
                var equipmentEntities = result.ToList();
                if (equipmentEntities.Any())
                {
                    return new OkObjectResult(equipmentEntities.FirstOrDefault());
                }
                else
                {
                    return new OkObjectResult(new EquipmentDto());
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(EquipmentDto), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Equipment"}, Summary = "Create new equipment")]
        public async Task<ActionResult> Create([FromBody] EquipmentDto item)
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

                var result = await _equipmentDomain.EquipmentUpd(item, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);

            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{equipmentGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Equipment"}, Summary = "Delete equipment")]
        public async Task<ActionResult> Delete(string equipmentGuid)
        {
            if (string.IsNullOrEmpty(equipmentGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await EquipmentInfo.EquipmentDel(equipmentGuid, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}