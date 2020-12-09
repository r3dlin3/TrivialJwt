using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt
{
    public class 
        TrivialJwtOptions
    {
        public const string Section = "TrivialJwt";


        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int AccessTokenLifetime { get; set; } = 3600;
        /// <summary>
        /// To prevent any clock synchronization issue, not before time will be equal to "now - clockskew"
        /// </summary>
        public int ClockSkew { get; set; } = 300;

        /// <summary>
        /// cf. https://tools.ietf.org/html/rfc7518#section-3
        /// </summary>
        public string SigningAlgorithm { get; set; } = "HS256";


        /// <summary>
        /// Define if the certificate
        /// Allowed values:  "File", "CertStore"
        /// </summary>
        public string CertificateStore { get; set; } = "File";
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }


        /// <summary>
        /// Define if the user is retrieved by mail, username or id.
        /// Allowed values:  "mail", "name" or "id"
        /// </summary>
        public string MethodRetrieval { get; set; }

        /// <summary>
        /// By default, issue a Refresh Token
        /// </summary>
        public bool IssueRefreshToken { get; set; } = true;

        /// <summary>
        /// Issue a new Refresh Token each time an access token is issued
        /// </summary>
        public bool EnableRefreshTokenRotation { get; set; }
        /// <summary>
        /// Validity of a refresh token for which an access token can be used in seconds
        /// By default, 8 hours.
        /// </summary>
        public int RefreshTokenLifetime { get; set; } = 28800;

        /// <summary>
        /// Maximum period of time a Refresh Token can be renewed in second
        /// By default, -1, which means there is no max time
        /// </summary>
        public int RefreshTokenMaxLifetime { get; set; } = -1;
    }
}
