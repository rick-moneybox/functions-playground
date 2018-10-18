using Functions.Accounts.Domain;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Functions.Accounts.Repostitory
{
    public class UserRepository
    {
        readonly DocumentClient _client;
        readonly Uri _databaseUri;
        readonly Uri _documentCollectionUri;

        public UserRepository(DocumentClient client)
        {
            _client = client;

            _databaseUri = UriFactory.CreateDatabaseUri("Authorization");
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri("Authorization", "Users");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            await InitializeDatabase();

            IDocumentQuery<UserState> query = _client.CreateDocumentQuery<UserState>(_documentCollectionUri)
                .Where(p => p.Email == email)
                .AsDocumentQuery();

            if (query.HasMoreResults)
            {
                foreach (UserState userState in await query.ExecuteNextAsync<UserState>())
                {
                    return new User(userState);
                }
            }

            return null;
        }

        public async Task<string> InsertUserAsync(User user)
        {
            await InitializeDatabase();

            var documentUri = UriFactory.CreateDocumentUri("Authorization", "Users", user.State.Id);

            var response = await _client.CreateDocumentAsync(
                _documentCollectionUri, 
                user.State,
                disableAutomaticIdGeneration: false);
            return response.Resource.Id;
        }

        public async Task InitializeDatabase()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database
            {
                Id = "Authorization"
            });

            await _client.CreateDocumentCollectionIfNotExistsAsync(
                _databaseUri, 
                new Microsoft.Azure.Documents.DocumentCollection
                {
                    Id = "Users"
                });
        }
    }
}
