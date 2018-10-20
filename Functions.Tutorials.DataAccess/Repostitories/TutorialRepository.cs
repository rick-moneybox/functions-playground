using Functions.Infrastructure;
using Functions.Tutorials.Core.Domain;
using Functions.Tutorials.Core.Repositories;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Functions.Accounts.DataAccess.Repositories
{
    public class TutorialRepository : ITutorialRepository
    {
        public static readonly string DatabaseName = "Moneybox";
        public static readonly string CollectionName = "Tutorials";

        readonly DocumentClient _client;
        readonly Uri _databaseUri;
        readonly Uri _documentCollectionUri;

        public TutorialRepository(DocumentClient client)
        {
            _client = client;

            _databaseUri = UriFactory.CreateDatabaseUri(DatabaseName);
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
        }

        public async Task<Tutorial> FindTutorial(string userId)
        {
            await InitializeDatabase();

            IDocumentQuery<TutorialState> query = _client.CreateDocumentQuery<TutorialState>(_documentCollectionUri)
                .Where(p => p.UserId == userId)
                .AsDocumentQuery();

            if (query.HasMoreResults)
            {
                foreach (TutorialState tutorialState in await query.ExecuteNextAsync<TutorialState>())
                {
                    return new Tutorial(tutorialState);
                }
            }

            return null;
        }

        public async Task<Tutorial> UpsertTutorial(Tutorial tutorial)
        {
            await InitializeDatabase();

            var response = await _client.UpsertDocumentAsync(
                _documentCollectionUri,
                tutorial.State,
                disableAutomaticIdGeneration: false);

            return tutorial;
        }

        async Task InitializeDatabase()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database
            {
                Id = DatabaseName
            });

            await _client.CreateDocumentCollectionIfNotExistsAsync(
                _databaseUri, 
                new Microsoft.Azure.Documents.DocumentCollection
                {
                    Id = CollectionName
                });
        }
    }
}
