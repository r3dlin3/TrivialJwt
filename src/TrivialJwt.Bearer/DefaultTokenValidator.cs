using Microsoft.IdentityModel.Tokens;
using TrivialJwt.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Bearer
{
    public class DefaultTokenValidator: ITokenValidator
    {
        private IIssuerService _issuerService;

        public DefaultTokenValidator(IIssuerService issuerService)
        {
            _issuerService = issuerService;
        }
        public string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken is null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }
            var expectedIssuer = _issuerService.GetIssuerUri();
            if (expectedIssuer!= securityToken.Issuer)
            {
                throw new SecurityTokenInvalidIssuerException($"Invalid issuer. Expected [{expectedIssuer}]. Was [{securityToken.Issuer}]");
            }
            return expectedIssuer;
        }

        public bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken is null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }
            if (audiences is null)
            {
                throw new ArgumentNullException(nameof(audiences));
            }

            var audienceList = new List<string>(audiences);
            if (audienceList.Count != 1)
                return false;
            string aud = audienceList[0];
            var expectedAudience = _issuerService.GetIssuerUri();
            return expectedAudience == aud;


        }
    }
}
