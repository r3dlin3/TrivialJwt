using Microsoft.AspNetCore.Mvc;
using SimpleJwt.Extensions;
using System;
using System.Text.Json;

using System.Threading.Tasks;

namespace SimpleJwt
{
    /// <summary>
    /// see IdentityServer4.Endpoints.Results.TokenResult
    /// </summary>
    internal class TokenResult : ContentResult
    {


        public TokenResult(TokenResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var dto = new ResultDto
            {
                id_token = response.IdentityToken,
                access_token = response.AccessToken,
                refresh_token = response.RefreshToken,
                expires_in = response.AccessTokenLifetime,
                token_type = "bearer",
                scope = response.Scope
            };
            ContentType = "application/json";
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            Content = JsonSerializer.Serialize<ResultDto>(dto, options);

        }
        public override void ExecuteResult(ActionContext context)
        {

            context.HttpContext.Response.SetNoCache();
            base.ExecuteResult(context);

        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {

            context.HttpContext.Response.SetNoCache();
            await base.ExecuteResultAsync(context);

        }

        internal class ResultDto
        {
            public string id_token { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }

        }
    }
}
