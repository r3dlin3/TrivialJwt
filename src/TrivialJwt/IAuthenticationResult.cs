using System;

namespace TrivialJwt
{
    public interface IAuthenticationResult
    {
        DateTime? AuthenticationTime();
        string GetUsername();
        bool IsError();
    }
}