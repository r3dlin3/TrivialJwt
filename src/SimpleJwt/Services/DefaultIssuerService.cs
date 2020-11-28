using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJwt.Services
{
    public class DefaultIssuerService : IIssuerService
    {
        private SimpleJwtOptions _options;
        private IHttpContextAccessor _contextAccessor;

        public DefaultIssuerService(
            IOptions<SimpleJwtOptions> options,
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
                var builder = new UriBuilder(request.Scheme, request.Host.Value);
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
