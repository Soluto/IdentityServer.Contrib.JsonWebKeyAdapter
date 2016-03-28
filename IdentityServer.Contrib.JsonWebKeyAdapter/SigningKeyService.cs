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
        private readonly ISigningService mSigningService;

        public SigningKeyService(ISigningService signingService)
        {
            mSigningService = signingService;
        }

        public Task<X509Certificate2> GetSigningKeyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<X509Certificate2>> GetPublicKeysAsync()
        {
            var publicKeys = await mSigningService.GetPublicKeysAsync();
            return publicKeys.Select(X509CertificateFactory.GenerateCertificate);
        }

        public Task<string> GetKidAsync(X509Certificate2 certificate)
        {
            return Task.FromResult(certificate.FriendlyName);
        }
    }
}
