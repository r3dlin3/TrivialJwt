using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using TrivialJwt.Services;
using TrivialJwt.Tests.Utils;
using Xunit;

namespace TrivialJwt.Tests
{
    public class DefaultTokenServiceUnitTests
    {
        [Fact]
        async Task TestTokenCreation()
        {
            var username = "test1";
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
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

        [Fact]
        async Task TestTokenCreationWithRSA()
        {
            var username = "test1";
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
                    {

                        SigningAlgorithm = "RS256",
                        CertificatePath = Path.Combine(System.AppContext.BaseDirectory, "certificate.p12"),
                        CertificatePassword = "1234"
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
