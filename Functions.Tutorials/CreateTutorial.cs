using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functions.Accounts.DataAccess.Repositories;
using Functions.Tutorials.Core.Domain;
using Functions.Tutorials.Core.Repositories;
using Functions.Tutorials.DataAccess.Repostitories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions.Tutorials
{
    public static class CreateTutorial
    {
        [FunctionName("CreateTutorial")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "Moneybox",
            collectionName: "Users",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [CosmosDB(
                databaseName: "Moneybox",
                collectionName: "Tutorials",
                ConnectionStringSetting = "CosmosDBConnection",
                CreateIfNotExists = true)] DocumentClient tutorialsClient,
            [CosmosDB(
                databaseName: "Moneybox",
                collectionName: "TutorialTips",
                ConnectionStringSetting = "CosmosDBConnection",
                CreateIfNotExists = true)] DocumentClient tutorialTipsClient,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                ITutorialRepository tutorialRepository = new TutorialRepository(tutorialsClient);
                ITutorialTipRepository tutorialTipRepository = new TutorialTipRepository(tutorialTipsClient);

                var tutorialTips = await tutorialTipRepository.GetAll();

                foreach (var id in input.Select(d => d.Id))
                {
                    var existingTutorial = await tutorialRepository.FindTutorial(id);

                    if (existingTutorial == null)
                    {
                        var tutorial = new Tutorial(id);
                        tutorial.SyncTutorialTips(tutorialTips.ToArray());

                        await tutorialRepository.UpsertTutorial(tutorial);
                    }
                }
            }
        }
    }
}
