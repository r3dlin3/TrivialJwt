using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TrivialJwt.Services
{
    public class DefaultTokenService : ITokenService
    {
        private readonly TrivialJwtOptions _options;
        private readonly ICredentialService _credentialService;
        private readonly IIssuerService _issuerService;
        private readonly ILogger<DefaultTokenService> _logger;

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DefaultTokenService(
            IOptions<TrivialJwtOptions> options,
            ICredentialService credentialService,
            IIssuerService issuerService, ILogger<DefaultTokenService> logger)
        {
            _options = options.Value;
            _credentialService = credentialService;
            _issuerService = issuerService;
            _logger = logger;
        }
        public async Task<string> GenerateTokenAsync(ClaimsIdentity user)
        {
            return await GenerateTokenAsync(user, DateTime.UtcNow);
        }

        public async Task<string> GenerateTokenAsync(ClaimsIdentity user, DateTime authTime)
        {
            return await generateAnyTokenAsync(user, authTime,
                tokenType: Constants.TokenType.AccessToken, lifeTime: _options.AccessTokenLifetime);
        }

        public async Task<string> GenerateRefreshTokenAsync(ClaimsIdentity user, DateTime authTime)
        {
            return await generateAnyTokenAsync(user, authTime,
                tokenType: Constants.TokenType.RefreshToken, lifeTime: _options.RefreshTokenLifetime);
        }

        private static long convertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        private async Task<string> generateAnyTokenAsync(
            ClaimsIdentity user,
            DateTime authTime,
            string tokenType,
            int lifeTime)
        {
            var credentials = await _credentialService.GetSigningCredentialsAsync();
            string issuer = _issuerService.GetIssuerUri();

            var claims = new Dictionary<string, object>
            {
                {JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()},
                {JwtRegisteredClaimNames.AuthTime, convertToTimestamp(authTime)},
                {Constants.ClaimType.TokenType, tokenType},
            };
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = user,
                Expires = DateTime.UtcNow.AddSeconds(lifeTime),
                NotBefore = DateTime.UtcNow.AddSeconds(-_options.ClockSkew),
                Issuer = issuer,
                Audience = issuer,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = credentials,
                Claims = claims,
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
