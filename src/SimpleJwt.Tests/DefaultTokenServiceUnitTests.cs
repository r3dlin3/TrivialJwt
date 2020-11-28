using Microsoft.Extensions.Options;
using SimpleJwt.Services;
using SimpleJwt.Tests.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleJwt.Tests
{
    public class DefaultTokenServiceUnitTests
    {
        [Fact]
        async Task TestTokenCreation()
        {
            var username = "test1";
            IOptions<SimpleJwtOptions> options = new OptionsWrapper<SimpleJwtOptions>(
                    new SimpleJwtOptions()
                    {
                        Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                        SigningAlgorithm = "HS256"
                    });
            var contextAccessor = HttpContextAccessorUtils.SetupHttpContextAccessorWithUrl("http://localhost:4000/auth/login");
            var defaultCredentialService = new DefaultCredentialService(options);
            DefaultIssuerService defaultIssuerService = new DefaultIssuerService(options, contextAccessor);
            var tokenService = new DefaultTokenService(options, defaultCredentialService, defaultIssuerService);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(Constants.ClaimTypes.Sub, username)
            });

            string token = await tokenService.GenerateTokenAsync(claimsIdentity);


            Assert.NotNull(token);

            var jwtToken = new JwtSecurityToken(token);
            Assert.Equal(username, jwtToken.Subject);
        }
    }
}
