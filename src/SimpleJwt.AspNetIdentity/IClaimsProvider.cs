﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SimpleJwt.AspNetIdentity
{
    public interface IClaimsProvider<TUser>
    {
        IList<Claim> GetClaims(TUser user);
    }
}
