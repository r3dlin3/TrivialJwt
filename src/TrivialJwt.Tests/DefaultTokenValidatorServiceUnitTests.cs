using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrivialJwt.Services;
using TrivialJwt.Tests.Utils;
using Xunit;

namespace TrivialJwt.Tests
{
    public class DefaultTokenValidatorServiceUnitTests
    {
        private DefaultTokenValidatorService generateDefaultTokenValidatorService(
            string secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
            string url = "http://localhost:4000/auth/login"
            )
        {

            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                new TrivialJwtOptions()
                {
                    Secret = secret,
                    SigningAlgorithm = "HS256"
                });

            return generateDefaultTokenValidatorService(options, secret, url);
        }

        private DefaultTokenValidatorService generateDefaultTokenValidatorService(
            IOptions<TrivialJwtOptions> options,
            string secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
            string url = "http://localhost:4000/auth/login"
            )
        {
            var contextAccessor = HttpContextAccessorUtils.SetupHttpContextAccessorWithUrl(url);
            var credentialService = new DefaultCredentialService(options);
            DefaultIssuerService issuerService = new DefaultIssuerService(options, contextAccessor);
            var logger = new Mock<ILogger<DefaultTokenValidatorService>>();


            DefaultTokenValidatorService validatorService = new DefaultTokenValidatorService(issuerService, credentialService, options, logger.Object);
            return validatorService;
        }

        [Fact]
        async Task TestInvalidAccessToken()
        {
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService();
            string token = await TokenUtils.GenerateToken(Constants.TokenType.AccessToken);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);
            Assert.True(authenticationResult.IsError());
        }

        [Fact]
        async Task TestValidRefreshToken()
        {
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService();
            string token = await TokenUtils.GenerateToken(Constants.TokenType.RefreshToken);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);
            Assert.False(authenticationResult.IsError());
            Assert.Equal(TokenUtils.USERNAME, authenticationResult.GetUsername());
        }

        [Fact]
        async Task TestInvalidSigningKeyRefreshToken()
        {
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService(
                secret: Convert.ToBase64String(Encoding.ASCII.GetBytes("NewSecret")));
            string token = await TokenUtils.GenerateToken(Constants.TokenType.RefreshToken);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);
            Assert.True(authenticationResult.IsError());
        }

        [Fact]
        async Task TestInvalidIssuerRefreshToken()
        {
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService(
                url: "https://trivialjwt.com/auth/refresh");
            string token = await TokenUtils.GenerateToken(Constants.TokenType.RefreshToken);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);
            Assert.True(authenticationResult.IsError());
        }

        [Fact]
        async Task TestInvalidMaxLifetimeRefreshToken()
        {
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                new TrivialJwtOptions()
                {
                    Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                    SigningAlgorithm = "HS256",
                    RefreshTokenLifetime = 3600,
                    RefreshTokenMaxLifetime = 8 * 3600
                });
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService(options);
            DateTime authTime = DateTime.UtcNow.AddHours(-8);
            string token = await TokenUtils.GenerateToken(Constants.TokenType.RefreshToken, authTime);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);
            Assert.True(authenticationResult.IsError());
        }

        [Fact]
        async Task TestValidMaxLifetimeRefreshToken()
        {
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                new TrivialJwtOptions()
                {
                    Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                    SigningAlgorithm = "HS256",
                    RefreshTokenLifetime = 3600,
                    RefreshTokenMaxLifetime = 8 * 3600
                });
            DefaultTokenValidatorService validatorService = generateDefaultTokenValidatorService(options);
            DateTime authTime = DateTime.UtcNow.AddHours(-6);
            string token = await TokenUtils.GenerateToken(Constants.TokenType.RefreshToken, authTime);

            IAuthenticationResult authenticationResult = await validatorService.ValidateRefreshTokenAsync(token);

            Assert.False(authenticationResult.IsError());
            Assert.Equal(TokenUtils.USERNAME, authenticationResult.GetUsername());
        }
    }
}
