using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TrivialJwt.Models;
using TrivialJwt.Services;
using System.Threading.Tasks;

namespace TrivialJwt.AspNetIdentity
{
    class PasswordValidator<TUser> : IPasswordValidator where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly UserManager<TUser> _userManager;
        private readonly IUserRetriever<TUser> _userRetriever;
        private readonly ILogger<PasswordValidator<TUser>> _logger;


        public PasswordValidator(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ILogger<PasswordValidator<TUser>> logger,
            IUserRetriever<TUser> userRetriever)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userRetriever = userRetriever;
        }
        public async Task<IAuthenticationResult> Validate(string username, string password)
        {
            // TODO: Define if email, username or Id

            var user = await _userRetriever.GetUserAsync(username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for username: {username}", username);

                    return new AuthenticationSuccess(sub);

                }
                else if (result.IsLockedOut)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", username);
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", username);
                }
                else
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", username);
                }
            }
            else
            {
                _logger.LogInformation("No user found matching username: {username}", username);
            }
            return new AuthenticationError();
        }
    }
}
