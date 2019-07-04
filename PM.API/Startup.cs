using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PM.API.Swagger;
using PM.InfrastructureModule.Data;
using PM.InfrastructureModule.Env;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PM.API
{
    [UsedImplicitly]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("AUTH_STS_MYSQL"),
                    mysqlOptions => { mysqlOptions.ServerVersion(new Version(8, 0, 12), ServerType.MySql); }
                ));

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = Env.StsPrimaryServer;
                    options.RequireHttpsMetadata = false;
                    // name of the API resource
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Env.StsPrimaryServer,
                        ValidateAudience = false,
                        ValidAudience = Env.AppAuthorize,
                        ValidateLifetime = true,
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Pixmake API", Version = "v1" });
                //c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}\\PM.InfrastructureModule.xml");
                c.EnableAnnotations();
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("AllRequests");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pixmake API V1");
                c.DocExpansion(DocExpansion.None);
            });
            app.UseMvc();
        }
    }
}
