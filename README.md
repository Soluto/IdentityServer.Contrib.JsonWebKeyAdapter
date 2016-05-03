# IdentityServer.Contrib.JsonWebKeyAdapter
A small library that allows working with JsonWebKey instead of X509Certificate.
This is useful when the key is stored in HSM and the private key cannot be extracted for example. In these cases, it is easier to represent the public key as JsonWebKey instead of X509Certificate.

Master build: [![Build status](https://ci.appveyor.com/api/projects/status/i3wf58puk4u6xkho/branch/master?svg=true)](https://ci.appveyor.com/project/omerlh/identityserver-contrib-jsonwebkeyadapter/branch/master)
##Getting Started:##
* Implement the ```IPublicKeyProvider``` interface.
* Register the ```ISigningKeyService``` that come with this library, and provide your ``IPublicKeyProvider`` implementation:

```csharp
    var factory = new IdentityServerServiceFactory();
    factory.SigningKeyService = new Registration<ISigningKeyService>(
                new SigningKeyService(<<Your implementation of IPublicKeyProvider>>));
    var options = new IdentityServerOptions
      {
        Factory = factory,
      };

    app.UseIdentityServer(options);
```

See the simple test in this repo for a complete exmaple.
Note that you have to override the identity server's default implementation of ```ITokenSigningService``` as it is using ```X509Certificate``` to sign the JWT.

## Contributing
Thanks for thinking about contributing! We are looking for contributions of any sort and size - features, bug fixes, documentation or anything else that you think will make it better.
* Fork and clone locally
* Create a topic specific branch
* Add a cool feature or fix a bug
* Add tests
* Send a Pull Request
