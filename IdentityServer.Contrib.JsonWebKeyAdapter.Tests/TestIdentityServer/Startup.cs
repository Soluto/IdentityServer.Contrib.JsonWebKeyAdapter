using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Owin;

namespace IdentityServer.Contrib.JsonWebKeyAdapter.Tests.TestIdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryClients(Clients.Get()).UseInMemoryScopes(Scopes.Get()).UseInMemoryUsers(Users.Get());
            var testSingingService = new TestPublicKeyProvider();
            factory.TokenSigningService = new Registration<ITokenSigningService>(new TokenSigningService(testSingingService));
            factory.SigningKeyService = new Registration<ISigningKeyService>(new SigningKeyService(testSingingService));
            var options = new IdentityServerOptions
            {
                RequireSsl = false,
                Factory = factory,
            };

            app.UseIdentityServer(options);
        }
    }
}