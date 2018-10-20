using Newtonsoft.Json;
using System;

namespace Functions.Accounts.Core.Domain
{
    public class User
    {
        private UserState _state;

        public UserState State =>
            JsonConvert.DeserializeObject<UserState>(JsonConvert.SerializeObject(_state));

        public User(UserState userState)
        {
            _state = userState;
        }

        public User(string name, string email, string password) 
            : this(new UserState())
        {
            _state.Id = Guid.NewGuid().ToString();
            _state.Email = email;
            _state.Name = name;

            // TODO: Hash
            _state.PasswordHash = password;
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
