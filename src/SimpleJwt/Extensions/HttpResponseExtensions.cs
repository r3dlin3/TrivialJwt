using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJwt.Extensions
{
    static class HttpResponseExtensions
    {
        public static async Task WriteJsonAsync(this HttpResponse response, object o, string contentType = null)
        {
            var json = ObjectSerializer.ToString(o);
            await response.WriteJsonAsync(json, contentType);
            await response.Body.FlushAsync();
        }

        public static async Task WriteJsonAsync(this HttpResponse response, string json, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json; charset=UTF-8";
            await response.WriteAsync(json);
            await response.Body.FlushAsync();
        }

        public static void SetCache(this HttpResponse response, int maxAge, params string[] varyBy)
        {
            if (maxAge == 0)
            {
                SetNoCache(response);
            }
            else if (maxAge > 0)
            {
                if (!response.Headers.ContainsKey("Cache-Control"))
                {
                    response.Headers.Add("Cache-Control", $"max-age={maxAge}");
                }

                if (varyBy?.Any() == true)
                {
                    var vary = varyBy.Aggregate((x, y) => x + "," + y);
                    if (response.Headers.ContainsKey("Vary"))
                    {
                        vary = response.Headers["Vary"].ToString() + "," + vary;
                    }
                    response.Headers["Vary"] = vary;
                }
            }
        }

        public static void SetNoCache(this HttpResponse response)
        {
            if (!response.Headers.ContainsKey("Cache-Control"))
            {
                response.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");
            }
            else
            {
                response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
            }

            if (!response.Headers.ContainsKey("Pragma"))
            {
                response.Headers.Add("Pragma", "no-cache");
            }
        }
    }
}
