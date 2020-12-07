using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrivialJwt.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

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
                // TODO handle config validation at startup
                if (string.IsNullOrEmpty(_options.Secret))
                    throw new InvalidOperationException("Secret is missing");
                byte[] key = Convert.FromBase64String(_options.Secret);
                _key = new SymmetricSecurityKey(key);
            }
            else
            {
                if (string.Compare(
                    Constants.CertificateStore.FileBased,
                    _options.CertificateStore, ignoreCase: true) == 0)
                {
                    if (string.IsNullOrEmpty(_options.CertificatePath))
                        throw new InvalidOperationException("Certificate path missing");
                    X509Certificate2 cert = new X509Certificate2(
                        _options.CertificatePath,
                        _options.CertificatePassword);


                    if (!cert.HasPrivateKey)
                        throw new InvalidOperationException("No private key");

                    if (SignatureAlgorithmHelper.IsRsa(_options.SigningAlgorithm)
                        && cert.PublicKey.Oid.FriendlyName == "RSA"
                        )
                    {
                        _key = new RsaSecurityKey(cert.GetRSAPrivateKey());
                    } else
                    {
                        throw new InvalidOperationException("Invalid Key algorithm for signing algorithm");
                    }
                } else
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
