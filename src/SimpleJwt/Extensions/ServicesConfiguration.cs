using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleJwt.Services;

namespace SimpleJwt.Extensions
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSimpleJwt(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Required for DefaultIssuerService
            services.AddScoped<IIssuerService, DefaultIssuerService>();
            services.AddScoped<ICredentialService, DefaultCredentialService>();
            services.AddScoped<ITokenService, DefaultTokenService>();
            services.AddMvcCore()
                .AddApplicationPart(Assembly.Load(typeof(ServicesConfiguration).Assembly.GetName()));   
            return services;
        }

        public static IServiceCollection AddSimpleJwt(this IServiceCollection services, Action<SimpleJwtOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddSimpleJwt();
        }
        public static IServiceCollection AddSimpleJwt(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SimpleJwtOptions>(config);
            return services.AddSimpleJwt();
        }
    }
}
