﻿using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace SimpleJwt.Bearer
{
    public interface ITokenValidator
    {
        bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);
        string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);
    }
}