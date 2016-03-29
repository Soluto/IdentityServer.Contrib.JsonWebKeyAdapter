using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;

namespace IdentityServer.Contrib.JsonWebKeyAdapter
{
    public interface ISigningService
    {
        Task<IEnumerable<JsonWebKey>> GetPublicKeysAsync();
    }
}