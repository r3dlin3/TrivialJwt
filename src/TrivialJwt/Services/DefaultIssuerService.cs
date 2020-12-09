using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Services
{
    public class DefaultIssuerService : IIssuerService
    {
        private TrivialJwtOptions _options;
        private IHttpContextAccessor _contextAccessor;

        public DefaultIssuerService(
            IOptions<TrivialJwtOptions> options,
            IHttpContextAccessor contextAccessor)
        {
            this._options = options.Value;
            this._contextAccessor = contextAccessor;
        }
        public string GetIssuerUri()
        {
            if (string.IsNullOrEmpty(_options.Issuer))
            {
                var request = _contextAccessor.HttpContext.Request;
                var builder = new UriBuilder(request.Scheme, request.Host.Host);
                if (request.Host.Port !=null)
                {
                    builder.Port = request.Host.Port.Value;
                }
                builder.Path = request.PathBase;
                return builder.ToString();
            }
            else
            {
                return _options.Issuer;
            }
        }
    }
}
