using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Container = Microsoft.Azure.Cosmos.Container;

namespace SiteCheckRobot.Core
{
    public class SiteHealthRepository : ISiteHealthRepository
    {
        private IConfiguration _configuration;

        public SiteHealthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Connect()
        {
            string connectionStrings = _configuration["CosmosConnectionStrings"];

            var cosmosClient = new CosmosClient(connectionStrings);


            string databaseName = _configuration["CosmosDb"];
            string containerName = _configuration["CosmosContainer"];
            Database database = cosmosClient.GetDatabase(databaseName);
            Container container = database.GetContainer(containerName);

            var item = new { id = Guid.NewGuid(), name = "MyItem" };
            var response = await container.CreateItemAsync(item);
        }
    }
}
