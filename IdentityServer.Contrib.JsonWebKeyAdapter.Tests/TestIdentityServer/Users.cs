using System.Collections.Generic;
using IdentityServer3.Core.Services.InMemory;

namespace IdentityServer.Contrib.JsonWebKeyAdapter.Tests.TestIdentityServer
{
    static class Users
    {
        private static readonly List<InMemoryUser> mUsers = new List<InMemoryUser>();

        public static List<InMemoryUser> Get()
        {
            return mUsers;
        }

        public static void AddUser(string userName)
        {
            mUsers.Add(new InMemoryUser
            {
                Enabled = true,
                Password = userName,
                Subject = userName,
                Username = userName
            });
        }
    }
}