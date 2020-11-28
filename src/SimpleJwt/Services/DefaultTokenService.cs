using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SimpleJwt.Services
{
    public class DefaultTokenService : ITokenService
    {
        private readonly SimpleJwtOptions _options;
        private readonly ICredentialService _credentialService;
        private readonly IIssuerService _issuerService;

        public DefaultTokenService(
            IOptions<SimpleJwtOptions> options,
            ICredentialService credentialService,
            IIssuerService issuerService)
        {
            _options = options.Value;
            _credentialService = credentialService;
            _issuerService = issuerService;
        }
        public async Task<string> GenerateTokenAsync(ClaimsIdentity user)
        {

            var credentials = await _credentialService.GetSigningCredentialsAsync();
            string issuer = _issuerService.GetIssuerUri();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = user,
                Expires = DateTime.UtcNow.AddSeconds(_options.AccessTokenLifetime),
                NotBefore = DateTime.UtcNow.AddSeconds(-_options.ClockSkew),
                Issuer = issuer,
                Audience = issuer,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

    }
}
