using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace IdentityServer.Contrib.JsonWebKeyAdapter.Tests.TestIdentityServer
{
    static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = Constants.StandardScopes.OpenId,
                    IncludeAllClaimsForUser = true,
                    Type = ScopeType.Identity
                },
            };
        }
    }
}