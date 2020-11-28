using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SimpleJwt.Models;
using SimpleJwt.Services;
using System;
using System.Threading.Tasks;

namespace SimpleJwt.AspNetIdentity
{
    public class UserRetriever<TUser> : IUserRetriever<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ILogger<UserRetriever<TUser>> _logger;
        private readonly SimpleJwtOptions _options;


        public UserRetriever(
            UserManager<TUser> userManager,
            SimpleJwtOptions options,
            ILogger<UserRetriever<TUser>> logger)
        {
            _userManager = userManager;
            _options = options;
            _logger = logger;
        }

        public async Task<TUser> GetUserAsync(string username)
        {
            string method = _options.MethodRetrieval;
            if (string.IsNullOrEmpty(method))
                throw new InvalidOperationException("MethodRetrieval is not provided");

            TUser user = null;
            switch (method.ToLowerInvariant())
            {
                case "name":
                    user = await _userManager.FindByNameAsync(username);
                    break;
                case "email":
                    if (_userManager.SupportsUserEmail)
                        user = await _userManager.FindByEmailAsync(username);
                    else
                        throw new InvalidOperationException("UserManager does not supports Email");
                    break;
                case "id":
                    user = await _userManager.FindByIdAsync(username);
                    break;
                default:
                    throw new InvalidOperationException("Invalid MethodRetrieval");
            }

            if (user == null)
            {
                _logger.LogInformation("No user found matching username: {username}", username);
            }
            return user;
        }

    }
}
