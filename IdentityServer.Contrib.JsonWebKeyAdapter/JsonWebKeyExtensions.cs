using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Protocols;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace IdentityServer.Contrib.JsonWebKeyAdapter
{
    public static  class JsonWebKeyExtensions
    {
        public static X509Certificate2 ToX509Certtificate(this JsonWebKey publicKey)
        {
            if (publicKey.Kty != "RSA" && publicKey.Kty != "RSA-HSM")
            {
                throw new NotImplementedException();
            }

            var kpgen = new RsaKeyPairGenerator();

            // certificate strength 1024 bits
            kpgen.Init(new KeyGenerationParameters(
                  new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var gen = new X509V3CertificateGenerator(); 

            var certName = new X509Name("CN=IdentityServer");
            var serialNo = BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(serialNo);
            gen.SetSubjectDN(certName);
            gen.SetIssuerDN(certName);
            gen.SetNotAfter(DateTime.Now.AddYears(100));
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            gen.SetSignatureAlgorithm("SHA1withRSA");
            var publicKeyParameters = new RsaKeyParameters(false, new BigInteger(1, Convert.FromBase64String(publicKey.N)), new BigInteger(1, Convert.FromBase64String(publicKey.E)));
            gen.SetPublicKey(publicKeyParameters);

            gen.AddExtension(
                X509Extensions.AuthorityKeyIdentifier.Id,
                false,
                new AuthorityKeyIdentifier(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKeyParameters),
                    new GeneralNames(new GeneralName(certName)),
                    serialNo));

            /* 
         1.3.6.1.5.5.7.3.1 - id_kp_serverAuth 
         1.3.6.1.5.5.7.3.2 - id_kp_clientAuth 
         1.3.6.1.5.5.7.3.3 - id_kp_codeSigning 
         1.3.6.1.5.5.7.3.4 - id_kp_emailProtection 
         1.3.6.1.5.5.7.3.5 - id-kp-ipsecEndSystem 
         1.3.6.1.5.5.7.3.6 - id-kp-ipsecTunnel 
         1.3.6.1.5.5.7.3.7 - id-kp-ipsecUser 
         1.3.6.1.5.5.7.3.8 - id_kp_timeStamping 
         1.3.6.1.5.5.7.3.9 - OCSPSigning
         */
            gen.AddExtension(
                X509Extensions.ExtendedKeyUsage.Id,
                false,
                new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));

            var newCert = gen.Generate(kpgen.GenerateKeyPair().Private);
            var certificate = new X509Certificate2(newCert.GetEncoded());
            certificate.SetKeyId(publicKey.Kid);
            return certificate;
        }
    }
}
