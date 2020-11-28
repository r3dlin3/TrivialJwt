using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleJwt.AspNetIdentity;
using SimpleJwt.Extensions;
using SimpleJwt.Services;

namespace SimpleJwt.AspNetIdentity
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSimpleJwtAspNetIdentity<TUser>(this IServiceCollection services) where TUser : IdentityUser
        {
            services.AddSimpleJwt();

            services.AddScoped<IUserRetriever<TUser>, UserRetriever<TUser>>();
            services.AddScoped<IPasswordValidator, PasswordValidator<TUser>>();
            services.AddScoped<IClaimsProvider<TUser>, DefaultClaimsProvider<TUser>>();
            services.AddScoped<IClaimsIdentityProvider, DefaultClaimsIdentityProvider<TUser>>();

            return services;
        }


        public static IServiceCollection AddSimpleJwtAspNetIdentity<TUser>(this IServiceCollection services, Action<SimpleJwtOptions> setupAction) where TUser : IdentityUser
        {
            services.Configure(setupAction);
            return services.AddSimpleJwtAspNetIdentity<TUser>();
        }
        public static IServiceCollection AddSimpleJwtAspNetIdentity<TUser>(this IServiceCollection services, IConfiguration config) where TUser : IdentityUser
        {
            services.Configure<SimpleJwtOptions>(config);
            return services.AddSimpleJwtAspNetIdentity<TUser>();
        }
    }
        
}
