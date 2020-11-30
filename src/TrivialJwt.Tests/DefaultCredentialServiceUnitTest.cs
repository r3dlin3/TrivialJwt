using System;
using Xunit;
using TrivialJwt;
using TrivialJwt.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace TrivialJwt.Tests
{
    public class DefaultCredentialServiceUnitTest
    {
        [Fact]
        public async Task TestSymmetricKey()
        {
            IOptions<TrivialJwtOptions> options = new OptionsWrapper<TrivialJwtOptions>(
                    new TrivialJwtOptions()
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
