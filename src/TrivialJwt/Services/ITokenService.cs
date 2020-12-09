using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    public interface ITokenService
    {
        Task<string> GenerateRefreshTokenAsync(ClaimsIdentity user, DateTime authTime);
        Task<string> GenerateTokenAsync(ClaimsIdentity user);
        Task<string> GenerateTokenAsync(ClaimsIdentity user, DateTime authTime);
    }
}
