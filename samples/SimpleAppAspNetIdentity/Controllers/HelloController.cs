using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleAppAspNetIdentity.Controllers
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
            return "Hello " + claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";
        }
    }
}
