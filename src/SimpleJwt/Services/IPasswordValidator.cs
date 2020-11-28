
using System.Threading.Tasks;

namespace SimpleJwt.Services
{
    public interface IPasswordValidator
    {
        Task<IAuthenticationResult> Validate(string username, string password);
    }
}
