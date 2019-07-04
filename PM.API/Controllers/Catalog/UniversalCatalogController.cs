using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Domain.Catalog;
using PM.InfrastructureModule.Domain.Services.Catalog;
using PM.InfrastructureModule.Dto.Catalog;
using PM.InfrastructureModule.Repository.StaticQuery.Catalog;
using PM.InfrastructureModule.Repository.StaticQuery.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Unity;

namespace PM.API.Controllers.Catalog
{
    [Route("api/v1/catalog/u")]
    [ApiController]
    [Authorize]
    public class UniversalCatalogController : Controller
    {
        [UsedImplicitly] private readonly ICatalogDomain _catalogDomain;

        public UniversalCatalogController()
        {
            _catalogDomain = Bootstraper.Init().Resolve<CatalogDomain>();
        }

        [HttpGet]
        [Route("{objectTypeName}")]
        [ProducesResponseType(typeof(List<object>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] { "Catalog" }, Summary = "Get requested type item list")]
        public async Task<ActionResult> SelectList(string objectTypeName)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _catalogDomain.CatalogGet(objectTypeName, null, serviceGroupGuid, null);

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{objectTypeName}/filter/{filterString}")]
        [ProducesResponseType(typeof(List<object>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Catalog"}, Summary = "Get requested type item list")]
        public async Task<ActionResult> SelectList( string objectTypeName, string filterString)
        {
            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _catalogDomain.CatalogGet(objectTypeName, null, serviceGroupGuid, filterString);

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{objectTypeName}/{objectGuid}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Catalog"}, Summary = "Get requested type item")]
        public async Task<ActionResult> Select(string objectTypeName, string objectGuid)
        {
            if (string.IsNullOrEmpty(objectGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _catalogDomain.CatalogGet(objectTypeName, objectGuid, serviceGroupGuid, null);
                var catalogEntities = result.ToList();
                if (catalogEntities.Any())
                {
                    return new OkObjectResult(catalogEntities.FirstOrDefault());
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

        [HttpPost]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Catalog"}, Summary = "Create new item of type")]
        public async Task<ActionResult> Create([FromBody] UniversalCatalogGenericDto item)
        {
            try
            {
                var user = HttpContext.User;
                var userGuid = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _catalogDomain.CatalogUpd(item.object_type_name, item.catalog_object.ToString(),
                    userGuid, serviceGroupGuid);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{objectTypeName}/{objectGuid}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Tags = new[] {"Catalog"}, Summary = "Delete item of type")]
        public async Task<ActionResult> Delete(string objectTypeName, string objectGuid)
        {
            if (string.IsNullOrEmpty(objectGuid))
            {
                return new BadRequestResult();
            }

            try
            {
                var user = HttpContext.User;
                var serviceGroupGuid =
                    await UserIdentitInfo.GetUserServiceGroup(user.FindFirstValue(ClaimTypes.NameIdentifier));
                var resultAny = await UniversalCatalogInfo.CatalogObjectGet(objectTypeName, objectGuid, serviceGroupGuid);
                var catalogEntities = resultAny.ToList();
                if (catalogEntities.Any())
                {

                    await UniversalCatalogInfo.CatalogObjectDel(objectGuid, serviceGroupGuid);
                    return new OkResult();
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
    }
}