using System;
using Xunit;
using SimpleJwt;
using SimpleJwt.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SimpleJwt.Tests
{
    public class DefaultCredentialServiceUnitTest
    {
        [Fact]
        public async Task TestSymmetricKey()
        {
            IOptions<SimpleJwtOptions> options = new OptionsWrapper<SimpleJwtOptions>(
                    new SimpleJwtOptions()
                    {
                        Secret = "YWFqc3FmanFzamtkamxxc2pkbA==",
                        SigningAlgorithm = "HS256"
                    });
            

            DefaultCredentialService defaultCredentialService = new DefaultCredentialService(options);
            var credentials = await defaultCredentialService.GetSigningCredentialsAsync();
            Assert.NotNull(credentials);
        }
    }
}
