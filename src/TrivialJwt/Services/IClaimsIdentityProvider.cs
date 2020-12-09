using System.Security.Claims;
using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    /// <summary>
    /// This interface is responsible to convert a User class into a ClaimsPrincipal
    /// All claims present in the ClaimsIdentity will be added to the token
    /// </summary>
    public interface IClaimsIdentityProvider
    {
        Task<ClaimsIdentity> CreateAsync(string subject);
    }
}