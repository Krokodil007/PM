﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PM.API.Swagger
{
    /// <summary>
    /// SwaggerAuthorizedMiddleware
    /// </summary>
    public class SwaggerAuthorizedMiddleware
    {

        private readonly RequestDelegate _next;

        public SwaggerAuthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger")
                && !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
