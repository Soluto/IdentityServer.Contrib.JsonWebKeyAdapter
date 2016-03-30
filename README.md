# IdentityServer.Contrib.JsonWebKeyAdapter
A small library that allows working with JsonWebKey instead of X509Certificate.
This is useful when the key is stored in HSM and the private key cannot be extracted for example. In these cases, it is easier to represent the public key as JsonWebKey instead of X509Certificate.

##Getting Started:##
Implement the ```ISigningService``` interface and register it:

```csharp
    var factory = new IdentityServerServiceFactory();
    var testSingingService = new TestSigningService();
    factory.SigningKeyService = new Registration<ISigningKeyService>(new SigningKeyService(testSingingService));
    var options = new IdentityServerOptions
      {
        RequireSsl = false,
        Factory = factory,
      };

    app.UseIdentityServer(options);
```

See the simple test in this repo for a complete exmaple.
Note that you have to override the identity server's default implementation of ```ITokenSigningService``` as the it is using ```X509Certificate``` to sign the JWT.
