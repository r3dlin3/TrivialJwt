using System.Threading.Tasks;

namespace TrivialJwt.AspNetIdentity
{
    public interface IUserRetriever<TUser> where TUser : class
    {
        Task<TUser> GetUserAsync(string username);
    }
}