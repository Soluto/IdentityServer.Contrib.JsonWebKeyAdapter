using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer3.Core.Services;

namespace IdentityServer.Contrib.JsonWebKeyAdapter
{
    public class SigningKeyService : ISigningKeyService
    {
        private readonly IPublicKeyProvider mPublicKeyProvider;

        public SigningKeyService(IPublicKeyProvider publicKeyProvider)
        {
            mPublicKeyProvider = publicKeyProvider;
        }

        public Task<X509Certificate2> GetSigningKeyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<X509Certificate2>> GetPublicKeysAsync()
        {
            var publicKeys = await mPublicKeyProvider.GetAsync();
            return publicKeys.Select(key => key.ToX509Certtificate());
        }

        public Task<string> GetKidAsync(X509Certificate2 certificate)
        {
            return Task.FromResult(certificate.GetKeyId());
        }
    }
}
