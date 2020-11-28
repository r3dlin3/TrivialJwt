using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace SimpleJwt.Services
{
    public interface ICredentialService
    {
        Task<SigningCredentials> GetSigningCredentialsAsync();
    }
}