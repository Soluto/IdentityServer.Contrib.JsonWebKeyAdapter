# IdentityServer.Contrib.JsonWebKeyAdapter
A small nuget package allow to work with JsonWebKey instead of X509Certificate.

##Getting Started:##
Implement the ```ISigningService``` interface. Than, register it:

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
Also notice that you have to override the default implementation of ```ITokenSigningService``` from identity server, as the default implementation is using ```X509Certificate``` in order to sign the JWT.
