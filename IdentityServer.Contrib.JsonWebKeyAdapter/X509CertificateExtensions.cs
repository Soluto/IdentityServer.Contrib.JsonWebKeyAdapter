using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Contrib.JsonWebKeyAdapter
{
    public static class X509CertificateExtensions
    {
        public static void SetKeyId(this X509Certificate2 certificate, string keyId)
        {
            certificate.FriendlyName = keyId;
        }

        public static string GetKeyId(this X509Certificate2 certificate)
        {
            return certificate.FriendlyName;
        }
    }
}