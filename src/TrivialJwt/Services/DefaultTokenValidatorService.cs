using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrivialJwt.Models;

namespace TrivialJwt.Services
{
    public class DefaultTokenValidatorService : ITokenValidatorService
    {
        private readonly TrivialJwtOptions _options;
        private readonly ICredentialService _credentialService;
        private readonly IIssuerService _issuerService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ILogger<DefaultTokenValidatorService> _logger;

        public DefaultTokenValidatorService(IIssuerService issuerService, ICredentialService credentialService, IOptions<TrivialJwtOptions> options, ILogger<DefaultTokenValidatorService> logger)
        {
            _issuerService = issuerService;
            _credentialService = credentialService;
            _options = options.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _logger = logger;
        }

        public async Task<IAuthenticationResult> ValidateRefreshTokenAsync(string refreshToken)
        {
            SecurityKey securityKey = await _credentialService.GetSecurityKeyAsync();
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidIssuer = _issuerService.GetIssuerUri(),
                ValidAudience = _issuerService.GetIssuerUri(),
                IssuerSigningKey = securityKey,
                ValidAlgorithms = new string[] { _options.SigningAlgorithm },
                ClockSkew = new TimeSpan(0, 0, _options.ClockSkew),
                TypeValidator = (string type, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                {
                    var jwt = securityToken as JwtSecurityToken;
                    var claim = jwt.Claims.FirstOrDefault(c => c.Type == Constants.ClaimType.TokenType);
                    if (claim == null || Constants.TokenType.RefreshToken != claim.Value)
                        throw new SecurityTokenInvalidTypeException("Invalid type");
                    return type;
                },

            };
            try
            {
                var principal = _jwtSecurityTokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var securityToken);
                // XXX Necessary??
                if (!(securityToken is JwtSecurityToken jwtSecurityToken))
                {
                    _logger.LogError("Invalid refresh token");
                    return new AuthenticationError();
                }

                var claim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.AuthTime);
                if (claim == null)
                {
                    _logger.LogError("Refresh token without auth_time");
                    return new AuthenticationError();
                }
                long timestamp = long.Parse(claim.Value);
                DateTime authTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;

                if (_options.RefreshTokenMaxLifetime > 0)
                {
                    if ((DateTime.UtcNow - authTime).TotalSeconds + _options.RefreshTokenLifetime > _options.RefreshTokenMaxLifetime)
                    {
                        _logger.LogError("Refresh token max life time reached");
                        return new AuthenticationError();
                    }
                }

                string sub = ((ClaimsIdentity)principal.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                return new AuthenticationSuccess(sub, authTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid refresh token");
            }
            return new AuthenticationError();

        }
    }
}
