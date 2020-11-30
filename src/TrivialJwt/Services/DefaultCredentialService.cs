using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrivialJwt.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrivialJwt.Services
{
    public class DefaultCredentialService : ICredentialService
    {
        private TrivialJwtOptions _options;
        private SecurityKey _key;

        public DefaultCredentialService(IOptions<TrivialJwtOptions> options)
        {
            this._options = options.Value;
            if (SignatureAlgorithmHelper.IsSymmetric(_options.SigningAlgorithm))
            {
                if (string.IsNullOrEmpty(_options.Secret))
                    throw new InvalidOperationException("Secret is missing");
                byte[] key = Convert.FromBase64String(_options.Secret);
                _key = new SymmetricSecurityKey(key);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            return new SigningCredentials(_key, _options.SigningAlgorithm);
        }

        public async Task<SecurityKey> GetSecurityKeyAsync()
        {
            return _key;
        }

    }
}
