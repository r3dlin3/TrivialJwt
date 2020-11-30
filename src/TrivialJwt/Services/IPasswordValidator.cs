
using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    public interface IPasswordValidator
    {
        Task<IAuthenticationResult> Validate(string username, string password);
    }
}
