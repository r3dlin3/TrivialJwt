namespace SimpleJwt
{
    public interface IAuthenticationResult
    {
        string GetUsername();
        bool IsError();
    }
}