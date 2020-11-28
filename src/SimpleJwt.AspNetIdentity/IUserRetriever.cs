using System.Threading.Tasks;

namespace SimpleJwt.AspNetIdentity
{
    public interface IUserRetriever<TUser> where TUser : class
    {
        Task<TUser> GetUserAsync(string username);
    }
}