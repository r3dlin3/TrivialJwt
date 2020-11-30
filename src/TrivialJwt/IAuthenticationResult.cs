namespace TrivialJwt
{
    public interface IAuthenticationResult
    {
        string GetUsername();
        bool IsError();
    }
}