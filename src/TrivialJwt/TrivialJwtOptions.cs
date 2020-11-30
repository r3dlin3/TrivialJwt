using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt
{
    public class TrivialJwtOptions
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
        /// Define if the user is retrieved by mail, username or id.
        /// Allowed values:  "mail", "name" or "id"
        /// </summary>
        public string MethodRetrieval { get; set; }




    }
}
