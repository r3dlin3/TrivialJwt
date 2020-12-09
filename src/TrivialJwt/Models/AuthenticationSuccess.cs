using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Models
{
    public class AuthenticationSuccess : IAuthenticationResult
    {
        private string _username;
        private DateTime? _authTime;
        public AuthenticationSuccess(string username, DateTime? authTime = null)
        {
            this._username = username;
            _authTime = authTime;
        }

        public DateTime? AuthenticationTime() => _authTime;

        public string GetUsername() => _username;
        public bool IsError() => false;
    }
}
