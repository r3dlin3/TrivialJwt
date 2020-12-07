using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt
{
    public static class Constants
    {
        public static class CertificateStore
        {
            public const string FileBased = "file";
            public const string StoreBased = "store";
            
        }
        public static class ClaimTypes
        {
            public const string Sub = "sub";
        }
    }
}
