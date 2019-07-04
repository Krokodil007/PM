using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Entity.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace PM.API.Controllers.Utils.DataStore
{
    [Route("api/v1/redis")]
    [ApiController]
    [Authorize]
    public class RedisController : Controller
    {
        [HttpGet]
        [Route("{key}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Get redis item value by key")]
        //[ClaimRequirement("Claim01", "TestClaim")]
        public async Task<ActionResult> Select(string key)
        {
            try
            {
                if (await DsRedis.ItemExistsAsync(key))
                {
                    var result = await DsRedis.ItemGetAsync(key);
                    return new OkObjectResult(result);
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RedisKeyValue), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Create new redis item")]
        public async Task<ActionResult> Create([FromBody] RedisKeyValue item)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            try
            {
                int.TryParse(item.key_expire, out var ttl);
                if (ttl > 0)
                {
                    await DsRedis.ItemSetAsync(item.key, item.value, ttl);
                }
                else
                {
                    await DsRedis.ItemSetAsync(item.key, item.value);
                }
                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [Route("{key}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Update existing redis item value by key")]
        public async Task<ActionResult> Update([FromBody] RedisKeyValue item)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            try
            {
                if (await DsRedis.ItemExistsAsync(item.key))
                {
                    int.TryParse(item.key_expire, out var ttl);
                    if (ttl > 0)
                    {
                        await DsRedis.ItemSetAsync(item.key, item.value, ttl);
                    }
                    else
                    {
                        await DsRedis.ItemSetAsync(item.key, item.value);
                    }

                    return new OkResult();
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [Route("{key}")]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Remove redis item by key")]
        public async Task<ActionResult> Delete(string key)
        {
            try
            {
                if (await DsRedis.ItemExistsAsync(key))
                {
                    await DsRedis.ItemRemoveAsync(key);
                    return new OkResult();
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}