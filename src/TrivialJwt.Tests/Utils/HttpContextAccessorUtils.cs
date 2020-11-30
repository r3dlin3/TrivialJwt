using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialJwt.Tests.Utils
{

    internal class HttpContextAccessorUtils
    {

        /// <summary>
        /// cf. https://stackoverflow.com/a/62992265
        /// </summary>
        public static IHttpContextAccessor SetupHttpContextAccessorWithUrl(string currentUrl)
        {
            Mock<IHttpContextAccessor> mock =
                new Mock<IHttpContextAccessor>(MockBehavior.Strict);
            var httpContext = new DefaultHttpContext();
            setRequestUrl(httpContext.Request, currentUrl);

            mock
              .SetupGet(accessor => accessor.HttpContext)
              .Returns(httpContext);
            
            static void setRequestUrl(HttpRequest httpRequest, string url)
            {
                UriHelper
                  .FromAbsolute(url, out var scheme, out var host, out var path, out var query,
                    fragment: out var _);

                httpRequest.Scheme = scheme;
                httpRequest.Host = host;
                httpRequest.Path = path;
                httpRequest.QueryString = query;
            }
            return mock.Object;
        }
    }
}
