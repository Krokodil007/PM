using Microsoft.AspNetCore.Builder;

namespace PM.API.Swagger
{
    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            //return builder.UseMiddleware<SwaggerAuthorizedMiddleware>();
            return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}