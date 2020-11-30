using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace SimpleJwt.Services
{
    public interface ICredentialService
    {
        Task<SecurityKey> GetSecurityKeyAsync();
        Task<SigningCredentials> GetSigningCredentialsAsync();
    }
}