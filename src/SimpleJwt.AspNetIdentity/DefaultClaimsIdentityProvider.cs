using Microsoft.AspNetCore.Identity;
using SimpleJwt.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleJwt.AspNetIdentity
{
    class DefaultClaimsIdentityProvider<TUser> : IClaimsIdentityProvider where TUser : IdentityUser
    {
        private UserManager<TUser> _userManager;
        private IClaimsProvider<TUser> _claimsProvider;

        public DefaultClaimsIdentityProvider(
            UserManager<TUser> userManager,
            IClaimsProvider<TUser> claimsProvider)
        {
            _userManager = userManager;
            _claimsProvider = claimsProvider;
        }
        public async Task<ClaimsIdentity> CreateAsync(string subject)
        {
            var identity = new ClaimsIdentity();

            var user = await _userManager.FindByIdAsync(subject);
            identity.AddClaim(new Claim(Constants.ClaimTypes.Sub, subject));

            identity.AddClaims(_claimsProvider.GetClaims(user));

            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, email));
                }
            }
            return identity;
        }
    }
}
