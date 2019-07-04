using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Order;
using PM.InfrastructureModule.Domain.Services.Order;
using PM.InfrastructureModule.Domain.Services.Shared;
using PM.InfrastructureModule.Domain.Shared;
using PM.InfrastructureModule.Dto.Order.In;
using PM.InfrastructureModule.Dto.Order.Out;
using PM.InfrastructureModule.Repository.StaticQuery.Order;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Unity;

namespace PM.API.Controllers.Order
{
    [Route("api/v1/order")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        [UsedImplicitly] private readonly IOrderDomain _orderDomain;
        [UsedImplicitly] private readonly IAttachmentDomain _attachmentDomain;

        public OrderController()
        {
            _orderDomain = Bootstraper.Init().Resolve<OrderDomain>();
            _attachmentDomain = Bootstraper.Init().Resolve<AttachmentDomain>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Get list of Orders")]
        public async Task<ActionResult> SelectList()
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.OrderGet(null, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{orderGuid}")]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Get Order")]
        public async Task<ActionResult> Select(string orderGuid)
        {
            if (string.IsNullOrEmpty(orderGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.OrderGet(orderGuid, serviceGroupGuid);
                var orderEntities = result.ToList();
                if (orderEntities.Any())
                {
                    return new OkObjectResult(orderEntities.FirstOrDefault());
                }
                else
                {
                    return new OkObjectResult(new OrderDto());
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Create new Order")]
        public async Task<ActionResult> Create([FromBody] OrderDto item)
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

                var result = await _orderDomain.OrderUpd(item, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);

            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [Route("{orderGuid}/approved/{approved}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Update Order approved status")]
        public async Task<ActionResult> UpdateApproved(string orderGuid, bool approved)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await OrderInfo.OrderApprovedUpd(orderGuid, approved, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{orderGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Delete Order")]
        public async Task<ActionResult> Delete(string orderGuid)
        {
            if (string.IsNullOrEmpty(orderGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await OrderInfo.OrderDel(orderGuid, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{orderGuid}/nomenclature")]
        [ProducesResponseType(typeof(List<NomenclatureOutDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Get list of Nomenclatures")]
        public async Task<ActionResult> NomenclatureSelectList(string orderGuid)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.NomenclatureGet(null, orderGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("nomenclature/{nomenclatureGuid}")]
        [ProducesResponseType(typeof(NomenclatureOutDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Get Nomenclature")]
        public async Task<ActionResult> NomenclatureSelect(string nomenclatureGuid)
        {
            if (string.IsNullOrEmpty(nomenclatureGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.NomenclatureGet(nomenclatureGuid, null, serviceGroupGuid);
                var orderEntities = result.ToList();
                if (orderEntities.Any())
                {
                    return new OkObjectResult(orderEntities.FirstOrDefault());
                }
                else
                {
                    return new OkObjectResult(new NomenclatureOutDto());
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("nomenclature")]
        [ProducesResponseType(typeof(List<NomenclatureOutDto>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Create/Update Nomenclature")]
        public async Task<ActionResult> NomenclatureCreate([FromBody] List<NomenclatureOutDto> itemList)
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
                var result = await _orderDomain.NomenclatureUpd(itemList, userGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("nomenclature")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Delete Nomenclature")]
        public async Task<ActionResult> NomenclatureDelete([FromBody] List<string> nomenclatureGuidList)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                await _orderDomain.NomenclatureDel(nomenclatureGuidList, serviceGroupGuid);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("nomenclature/calc/form1")]
        [ProducesResponseType(typeof(NomenclatureOutDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Calculate Nomenclature Form 1")]
        public async Task<ActionResult> NomenclatureCalculateForm1([FromBody] NomenclatureForm1InDto item)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.NomenclatureCalculateForm1(item, serviceGroupGuid);

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("nomenclature/calc/form3")]
        [ProducesResponseType(typeof(NomenclatureOutDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Calculate Nomenclature Form 3")]
        public async Task<ActionResult> NomenclatureCalculateForm3([FromBody] NomenclatureForm3InDto item)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _orderDomain.NomenclatureCalculateForm3(item, serviceGroupGuid);

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("calc")]
        [ProducesResponseType(typeof(OrderFullDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Order"}, Summary = "Calculate discount/markup for order")]
        public async Task<ActionResult> OrderDiscountMarkupCalculate([FromBody] OrderFullDto order)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = _orderDomain.OrderDiscountMarkupCalculate(order);

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}