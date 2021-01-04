using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrivialJwt.Models;
using TrivialJwt.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace TrivialJwt.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordValidator _passwordChecker;
        private readonly IClaimsIdentityProvider _claimsIdentityProvider;
        private readonly ITokenService _tokenService;
        private readonly TrivialJwtOptions _options;
        private readonly ITokenValidatorService _tokenValidator;


        public AuthController(IPasswordValidator passwordChecker,
            IClaimsIdentityProvider claimsIdentityProvider,
            ITokenService tokenService, IOptions<TrivialJwtOptions> options, ITokenValidatorService tokenValidator)
        {
            _passwordChecker = passwordChecker;
            _claimsIdentityProvider = claimsIdentityProvider;
            _tokenService = tokenService;
            _options = options.Value;
            _tokenValidator = tokenValidator;
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TokenResult.ResultDto), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            IAuthenticationResult result = await _passwordChecker.Validate(model.Username, model.Password);
            if (result.IsError())
                return new UnauthorizedResult();

            ClaimsIdentity user = await _claimsIdentityProvider.CreateAsync(result.GetUsername());
            string token = await _tokenService.GenerateTokenAsync(user);

            var response = new TokenResponse()
            {
                AccessToken = token,
                AccessTokenLifetime = _options.AccessTokenLifetime,
            };

            if (_options.IssueRefreshToken)
            {
                response.RefreshToken = await _tokenService.GenerateRefreshTokenAsync(user, DateTime.UtcNow);
            }

            return new TokenResult(response);
        }

        [HttpPost("refresh_token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TokenResult.ResultDto), 200)]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            if (!_options.IssueRefreshToken)
                return BadRequest();

            IAuthenticationResult result = await _tokenValidator.ValidateRefreshTokenAsync(model.refresh_token);
            if (result.IsError())
                return new UnauthorizedResult();

            ClaimsIdentity user = await _claimsIdentityProvider.CreateAsync(result.GetUsername());
            string token = await _tokenService.GenerateTokenAsync(user);
            string refreshToken = await _tokenService.GenerateTokenAsync(user);
            var response = new TokenResponse()
            {
                AccessToken = token,
                AccessTokenLifetime = _options.AccessTokenLifetime,
                RefreshToken = refreshToken
            };
            return new TokenResult(response);
        }
    }
}
