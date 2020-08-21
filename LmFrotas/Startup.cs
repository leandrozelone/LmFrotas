using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LmFrotas.Service;
using LmFrotas.Service.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LmFrotas
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();          
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto teste .Net Core LmFrotas",
                    Description = "Projeto simples para estudo",
                    Contact = new OpenApiContact
                    {
                        Name = "Leandro Zelone",
                        Email = "leandro.zelone@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/leandro-zelone/"),
                    },
                });
            });

            services.AddScoped<ITokenService, TokenService>();

            var SecretKeyJwt = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKeyJwt"));

            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer (options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "LMFrotas",
                    ValidAudience = "LMFrotas",
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKeyJwt)
                };

                        Console.WriteLine(_configuration.GetValue<string>("SecretKeyJwt"));

                options.Events = new JwtBearerEvents {
                    OnAuthenticationFailed = context => {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context => {
                            return Task.CompletedTask;
                        }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
