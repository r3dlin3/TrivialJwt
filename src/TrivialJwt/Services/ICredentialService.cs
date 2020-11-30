using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    public interface ICredentialService
    {
        Task<SecurityKey> GetSecurityKeyAsync();
        Task<SigningCredentials> GetSigningCredentialsAsync();
    }
}