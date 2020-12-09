using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrivialJwt.Services;

namespace TrivialJwt.Tests.Utils
{
    static class TokenUtils
    {
        public const string USERNAME = "test1";
        public static async Task<string> GenerateToken(string type, DateTime? authTime = null)
        {
            if (!authTime.HasValue)
                authTime = DateTime.UtcNow;

            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, USERNAME)
            });
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
                    {
                        Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                        SigningAlgorithm = "HS256"
                    });
            var contextAccessor = HttpContextAccessorUtils.SetupHttpContextAccessorWithUrl("http://localhost:4000/auth/login");
            var defaultCredentialService = new DefaultCredentialService(options);
            DefaultIssuerService defaultIssuerService = new DefaultIssuerService(options, contextAccessor);
            var logger = new Mock<ILogger<DefaultTokenService>>();
            var tokenService = new DefaultTokenService(options,
                    defaultCredentialService,
                    defaultIssuerService,
                    logger.Object);

            string token;
            if (type == Constants.TokenType.AccessToken)
                token = await tokenService.GenerateTokenAsync(claimsIdentity, authTime.Value);
            else
                token = await tokenService.GenerateRefreshTokenAsync(claimsIdentity, authTime.Value);
            return token;
        }
    }
}
