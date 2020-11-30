using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace SimpleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Hello()
        {
            if (User == null || !User.Identity.IsAuthenticated)
                return "Hello World";
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            return "Hello " + claimsIdentity.Claims.FirstOrDefault(c=>c.Type=="name")?.Value ?? "";
        }
    }
}
