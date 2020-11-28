using SimpleJwt;
using SimpleJwt.Models;
using SimpleJwt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApp
{
    public class PasswordValidator : IPasswordValidator
    {
        public async Task<IAuthenticationResult> Validate(string username, string password)
        {
            if (username == password)
            {
                return new AuthenticationSuccess(username);
            }

            return new AuthenticationError();
        }
    }
}
