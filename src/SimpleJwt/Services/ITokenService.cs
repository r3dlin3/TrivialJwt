using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJwt.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ClaimsIdentity user);
    }
}
