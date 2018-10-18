using Newtonsoft.Json;
using System;

namespace Functions.Accounts.Core.Domain
{
    public class User
    {
        public UserState State { get; }

        public User(UserState userState)
        {
            State = userState;
        }

        public User(string name, string email, string password) 
            : this(new UserState())
        {
            State.Id = Guid.NewGuid().ToString();
            State.Email = email;
            State.Name = name;

            // TODO: Hash
            State.PasswordHash = password;
        }
    }

    public class UserState
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
