using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrivialJwt.Services;
using TrivialJwt.Tests.Utils;
using Xunit;

namespace TrivialJwt.Tests
{
    public class DefaultTokenServiceUnitTests
    {
        public const string ISSUER = "http://localhost:4000/";

        public readonly IOptions<TrivialJwtOptions> OPTIONS = new OptionsWrapper<TrivialJwtOptions>(
        new TrivialJwtOptions()
        {
            Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
            SigningAlgorithm = "HS256"
        });

        private void validateToken(string token, string username, string issuer)
        {
            Assert.NotNull(token);

            var jwtToken = new JwtSecurityToken(token);
            Assert.Equal(username, jwtToken.Subject);
            Assert.NotEmpty(jwtToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Jti));
            Assert.NotEmpty(jwtToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Iat));
            Assert.NotEmpty(jwtToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Iss));
            Assert.Equal(issuer, jwtToken.Issuer);
            Assert.NotEmpty(jwtToken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Aud));
            Assert.Equal(issuer, jwtToken.Audiences.First());
        }

        private async Task<(string token, string username)> generateToken()
        {
            return await generateToken(OPTIONS);
        }
        private async Task<(string token, string username)> generateToken(IOptions<TrivialJwtOptions> options, string baseUrl = ISSUER)
        {
            var username = Guid.NewGuid().ToString();

            var contextAccessor = HttpContextAccessorUtils.SetupHttpContextAccessorWithUrl(baseUrl + "auth/login");
            var defaultCredentialService = new DefaultCredentialService(options);
            DefaultIssuerService defaultIssuerService = new DefaultIssuerService(options, contextAccessor);
            var logger = new Mock<ILogger<DefaultTokenService>>();
            var tokenService = new DefaultTokenService(options,
                    defaultCredentialService,
                    defaultIssuerService,
                    logger.Object);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, username)
           });
            string token = await tokenService.GenerateTokenAsync(claimsIdentity);

            return (token, username);
        }

        [Fact]
        async Task TestTokenCreation()
        {
            (string token, string username) = await generateToken();
            validateToken(token, username, ISSUER);
        }

        [Fact]
        async Task TestTokenCreationWithCustomIssuer()
        {
            string issuer = "https://trivialjwt.com";
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
                    {
                        Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                        SigningAlgorithm = "HS256",
                        Issuer = issuer
                    }); ;

            (string token, string username) = await generateToken(options);
            validateToken(token, username, issuer);
        }

        [Fact]
        async Task TestTokenCreationContextWithoutPort()
        {
            string baseUrl = "https://trivialjwt.com/";
            (string token, string username) = await generateToken(OPTIONS, baseUrl);
            validateToken(token, username, baseUrl);
        }

        [Fact]
        async Task TestRefreshTokenCreation()
        {
            var username = Guid.NewGuid().ToString();

            var contextAccessor = HttpContextAccessorUtils.SetupHttpContextAccessorWithUrl(ISSUER  + "/auth/login");
            var defaultCredentialService = new DefaultCredentialService(OPTIONS);
            DefaultIssuerService defaultIssuerService = new DefaultIssuerService(OPTIONS, contextAccessor);
            var logger = new Mock<ILogger<DefaultTokenService>>();
            var tokenService = new DefaultTokenService(OPTIONS,
                    defaultCredentialService,
                    defaultIssuerService,
                    logger.Object);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username)
            });

            string token = await tokenService.GenerateRefreshTokenAsync(claimsIdentity, DateTime.UtcNow);
            validateToken(token, username, ISSUER);
        }


        [Fact]
        async Task TestTokenCreationWithRSA()
        {
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
                    {

                        SigningAlgorithm = "RS256",
                        CertificatePath = Path.Combine(System.AppContext.BaseDirectory, "certificate.p12"),
                        CertificatePassword = "1234"
                    });
            (string token, string username) = await generateToken(options);
            validateToken(token, username, ISSUER);
        }
    }
}
