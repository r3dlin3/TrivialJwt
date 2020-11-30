using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrivialJwt.AspNetIdentity;
using TrivialJwt.Extensions;
using TrivialJwt.Services;

namespace TrivialJwt.AspNetIdentity
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddTrivialJwtAspNetIdentity<TUser>(this IServiceCollection services) where TUser : IdentityUser
        {
            services.AddTrivialJwt();

            services.AddScoped<IUserRetriever<TUser>, UserRetriever<TUser>>();
            services.AddScoped<IPasswordValidator, PasswordValidator<TUser>>();
            services.AddScoped<IClaimsProvider<TUser>, DefaultClaimsProvider<TUser>>();
            services.AddScoped<IClaimsIdentityProvider, DefaultClaimsIdentityProvider<TUser>>();

            return services;
        }


        public static IServiceCollection AddTrivialJwtAspNetIdentity<TUser>(this IServiceCollection services, Action<TrivialJwtOptions> setupAction) where TUser : IdentityUser
        {
            services.Configure(setupAction);
            return services.AddTrivialJwtAspNetIdentity<TUser>();
        }
        public static IServiceCollection AddTrivialJwtAspNetIdentity<TUser>(this IServiceCollection services, IConfiguration config) where TUser : IdentityUser
        {
            services.Configure<TrivialJwtOptions>(config);
            return services.AddTrivialJwtAspNetIdentity<TUser>();
        }
    }
        
}
