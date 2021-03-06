﻿using Functions.Accounts.Core.Domain;
using Functions.Accounts.Core.Repositories;
using Functions.Infrastructure;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Functions.Accounts.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public static readonly string DatabaseName = "Moneybox";
        public static readonly string CollectionName = "Users";

        readonly DocumentClient _client;
        readonly Uri _databaseUri;
        readonly Uri _documentCollectionUri;

        public UserRepository(DocumentClient client)
        {
            _client = client;

            _databaseUri = UriFactory.CreateDatabaseUri(DatabaseName);
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
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

        public async Task<User> InsertUserAsync(User user)
        {
            await InitializeDatabase();

            var documentUri = UriFactory.CreateDocumentUri(DatabaseName, CollectionName, user.State.Id);

            var response = await _client.CreateDocumentAsync(
                _documentCollectionUri, 
                user.State,
                disableAutomaticIdGeneration: false);

            return user;
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
