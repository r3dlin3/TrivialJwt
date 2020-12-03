using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrivialJwt.Services;

namespace TrivialJwt.Bearer
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddTrivialJwtAuthentication(this IServiceCollection services)
        {
            services.AddScoped<ITokenValidator, DefaultTokenValidator>(); 
            
            var scopeFactory = services
                    .BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var issuerValidator = scope.ServiceProvider.GetRequiredService<ITokenValidator>();
                var credentialService = scope.ServiceProvider.GetRequiredService<ICredentialService>();
                SecurityKey securityKey = credentialService.GetSecurityKeyAsync().Result;
                services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerValidator = issuerValidator.IssuerValidator,
                        AudienceValidator = issuerValidator.AudienceValidator,
                        IssuerSigningKey = securityKey
                    };
                });
            }

            return services;
        }
    }
}
