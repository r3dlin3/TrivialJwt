using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleJwt.Models;
using SimpleJwt.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJwt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly  IPasswordValidator _passwordChecker;
        private readonly  IClaimsIdentityProvider _claimsIdentityProvider;
        private readonly  ITokenService _tokenService;

        public AuthController(IPasswordValidator passwordChecker, IClaimsIdentityProvider claimsIdentityProvider, ITokenService tokenService)
        {
            _passwordChecker = passwordChecker;
            _claimsIdentityProvider = claimsIdentityProvider;
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            IAuthenticationResult result = await _passwordChecker.Validate(model.Username, model.Password);
            if (result.IsError())
                return new UnauthorizedResult();

            ClaimsIdentity user = await _claimsIdentityProvider.CreateAsync(result.GetUsername());
            string token = await _tokenService.GenerateTokenAsync(user);
            var response = new TokenResponse() { AccessToken = token };
            return new TokenResult(response);
        }
    }
}
