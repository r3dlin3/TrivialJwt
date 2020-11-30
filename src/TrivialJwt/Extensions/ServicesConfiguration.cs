using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrivialJwt.Services;

namespace TrivialJwt.Extensions
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddTrivialJwt(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Required for DefaultIssuerService
            services.AddScoped<IIssuerService, DefaultIssuerService>();
            services.AddScoped<ICredentialService, DefaultCredentialService>();
            services.AddScoped<ITokenService, DefaultTokenService>();
            services.AddMvcCore()
                .AddApplicationPart(Assembly.Load(typeof(ServicesConfiguration).Assembly.GetName()));   
            return services;
        }

        public static IServiceCollection AddTrivialJwt(this IServiceCollection services, Action<TrivialJwtOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddTrivialJwt();
        }
        public static IServiceCollection AddTrivialJwt(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<TrivialJwtOptions>(config);
            return services.AddTrivialJwt();
        }
    }
}
