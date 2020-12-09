using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using TrivialJwt;
using TrivialJwt.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using TrivialJwt.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TrivialJwt.Tests
{
    public class ControllerIntegrationTests
    {
        

        [Fact]
        public async Task TestValidAuthentication()
        {
            var factory = new WebApplicationFactory<SimpleApp.Startup>();

            var client = factory.CreateClient();

            var authInfo = new LoginModel()
            {
                Username = "bob",
                Password = "bob"
            };
            string json = JsonConvert.SerializeObject(authInfo);

            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/auth/login", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jToken = JToken.Parse(responseString);
            Assert.NotNull(jToken.access_token);
            Assert.NotNull(jToken.refresh_token);
        }

        [Fact]
        public async Task TestInvalidAuthentication()
        {
            var factory = new WebApplicationFactory<SimpleApp.Startup>();

            var client = factory.CreateClient();

            var authInfo = new LoginModel()
            {
                Username = "bob",
                Password = "bob2"
            };
            string json = JsonConvert.SerializeObject(authInfo);

            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/auth/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task TestRefreshToken()
        {
            var factory = new WebApplicationFactory<SimpleApp.Startup>();

            var client = factory.CreateClient();

            var authInfo = new LoginModel()
            {
                Username = "bob",
                Password = "bob"
            };
            string json = JsonConvert.SerializeObject(authInfo);

            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/auth/login", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jToken = JToken.Parse(responseString);
            Assert.NotNull(jToken.access_token);
            Assert.NotNull(jToken.refresh_token);


            RefreshTokenModel refreshTokenModel = new RefreshTokenModel()
            {
                refresh_token = jToken.refresh_token
            };
            json = JsonConvert.SerializeObject(refreshTokenModel);

            content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            response = await client.PostAsync("/auth/refresh_token", content);

            Assert.True(response.IsSuccessStatusCode);
            responseString = await response.Content.ReadAsStringAsync();
            jToken = JToken.Parse(responseString);
            Assert.NotNull(jToken.access_token);
            Assert.NotNull(jToken.refresh_token);



        }


    }
}
