using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Models
{
    public class AuthenticationSuccess : IAuthenticationResult
    {
        private string username;
        public AuthenticationSuccess(string username)
        {
            this.username = username;
        }

        public string GetUsername() => username;
        public bool IsError() => false;
    }
}
