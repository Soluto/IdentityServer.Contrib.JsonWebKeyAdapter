using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer.Contrib.JsonWebKeyAdapter.Tests.TestIdentityServer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.Jwt;
using NUnit.Framework;

namespace IdentityServer.Contrib.JsonWebKeyAdapter.Tests
{
    [TestFixture]
    public class TokenValidationTests
    {
        class Token
        {
            public string access_token { get; set; }
        }

        class Provider : IIssuerSecurityTokenProvider
        {
            public Provider(IEnumerable<JsonWebKey> keys)
            {
                SecurityTokens = from key in keys
                                 select new X509SecurityToken(new X509Certificate2(Convert.FromBase64String(key.X5c.First())), key.Kid);
            }

            public string Issuer { get { return "http://localhost"; } }
            public IEnumerable<SecurityToken> SecurityTokens { get; private set; }
        }

        [Test]
        public async Task RequestToken_ValidateUsingExposedPublicKey_ValidationSucceded()
        {
            var mockServer = new MockIdentityServer();
            var token = await mockServer.GetTokenForUser("blah");

            var valParams = new TokenValidationParameters{IssuerSigningKeyResolver =
                (t, securityToken, keyIdentifier, validationParameters) =>
                {
                    var kid =
                        keyIdentifier.OfType<NamedKeySecurityKeyIdentifierClause>()
                            .Where(identifier => identifier.Name.Equals("kid"))
                            .Select(identifier => identifier.Id)
                            .Single();
                    return validationParameters.IssuerSigningTokens.Single(key => key.Id == kid).SecurityKeys.First();
                },
            ValidAudience = "http://localhost/resources"};

            var format = new JwtFormat(valParams, new Provider((await mockServer.GetPublicKeys()).Keys));
            format.Unprotect(token);
        }
    }
}
