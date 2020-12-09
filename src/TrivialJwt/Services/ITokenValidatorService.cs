using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    public interface ITokenValidatorService
    {
        Task<IAuthenticationResult> ValidateRefreshTokenAsync(string refreshToken);
    }
}