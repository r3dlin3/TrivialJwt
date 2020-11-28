using System;

namespace SimpleJwt.Models
{
    public class AuthenticationError : IAuthenticationResult
    {
        public string GetUsername()
        {
            throw new InvalidOperationException();
        }

        public bool IsError() => true;
    }
}
