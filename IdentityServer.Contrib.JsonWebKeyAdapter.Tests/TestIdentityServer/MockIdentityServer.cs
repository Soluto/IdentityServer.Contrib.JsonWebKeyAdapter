using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Testing;

namespace IdentityServer.Contrib.JsonWebKeyAdapter.Tests.TestIdentityServer
{
    public class MockIdentityServer : IDisposable
    {
        private readonly TestServer mTestServer;

        public MockIdentityServer()
        {
            mTestServer = TestServer.Create<Startup>();
        }

        public async Task<JsonWebKeySet> GetPublicKeys()
        {
            var response = await mTestServer.HttpClient.GetAsync(".well-known/jwks");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<JsonWebKeySet>();
        }

        public async Task<string> GetTokenForUser(string userName)
        {
            Users.AddUser(userName);

            var tokenResponse = await mTestServer.HttpClient.PostAsync("/connect/token",
                    new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("client_id", "test"),
                            new KeyValuePair<string, string>("client_secret", "F621F470-9731-4A25-80EF-67A6F7C5F4B8"),
                            new KeyValuePair<string, string>("scope", "openid"),
                            new KeyValuePair<string, string>("username", userName),
                            new KeyValuePair<string, string>("password", userName),
                        }));

            tokenResponse.EnsureSuccessStatusCode();

            var token = await tokenResponse.Content.ReadAsAsync<TokenResponse>();

            return token.access_token;
        }

        public void Dispose()
        {
            if (mTestServer != null)
            {
                mTestServer.Dispose();
            }
        }
    }
}
