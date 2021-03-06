﻿using TrivialJwt;
using TrivialJwt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace SimpleApp
{
    public class ClaimsIdentityProvider : IClaimsIdentityProvider
    {
        public async Task<ClaimsIdentity> CreateAsync(string subject) => new ClaimsIdentity(new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim("name", subject)
            });
    }
}
