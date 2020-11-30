using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Helpers
{
    public static class SignatureAlgorithmHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwa">String identifying the algorithm. Based on https://tools.ietf.org/html/rfc7518#section-3 </param>
        public static string ConvertJWAToSignatureAlgorithm(string jwa)
        {
            return null;
        }

        public static bool IsSymmetric(string algo)
        {
            if (string.IsNullOrEmpty(algo))
                throw new ArgumentException("Algorithm is missing", nameof(algo));

            switch (algo) {
                case "HS256":
                case "HS384":
                case "HS512":
                    return true;
                case "RS256":
                case "RS512":
                case "ES256":
                case "ES384":
                case "ES512":
                case "PS256":
                case "PS384":
                case "PS512":
                    return false;
                default:
                    throw new ArgumentException("Unknown algorithm", nameof(algo));


            }

        }
    }
}
