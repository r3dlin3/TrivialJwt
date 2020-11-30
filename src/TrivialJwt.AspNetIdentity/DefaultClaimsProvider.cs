using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TrivialJwt.AspNetIdentity
{
    class DefaultClaimsProvider<TUser> : IClaimsProvider<TUser> where TUser: IdentityUser
    {
        public IList<Claim> GetClaims(TUser user) => new List<Claim>();
    }
}
