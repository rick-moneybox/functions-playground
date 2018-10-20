using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Functions.Tutorials.Core.Domain;
using Functions.Tutorials.Core.Repositories;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Functions.Tutorials.DataAccess.Repostitories
{
    public class TutorialTipRepository : ITutorialTipRepository
    {
        public static readonly string DatabaseName = "Moneybox";
        public static readonly string CollectionName = "TutorialTips";

        readonly DocumentClient _client;
        readonly Uri _databaseUri;
        readonly Uri _documentCollectionUri;

        public TutorialTipRepository(DocumentClient client)
        {
            _client = client;

            _databaseUri = UriFactory.CreateDatabaseUri(DatabaseName);
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
        }

        public async Task<List<TutorialTip>> GetAll()
        {
            await InitializeDatabase();

            IDocumentQuery<TutorialTipState> query = _client.CreateDocumentQuery<TutorialTipState>(_documentCollectionUri)
                .AsDocumentQuery();

            var tutorialTips = new List<TutorialTip>();
            if (query.HasMoreResults)
            {
                foreach (TutorialTipState state in await query.ExecuteNextAsync<TutorialTipState>())
                {
                    tutorialTips.Add(new TutorialTip(state));
                }
            }

            return tutorialTips;
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
