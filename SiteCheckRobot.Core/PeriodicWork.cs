using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SiteCheckRobot.Core
{
    public class PeriodicWork : IPeriodicWork
    {
        private readonly ILogger<PeriodicWork> _logger;

        private readonly IConfiguration _configuration;

        public PeriodicWork(ILogger<PeriodicWork> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Execute()
        {
            var siteCheck = new SiteCheck();
            var url = "https://www.matthias-jost.ch";
            await siteCheck.LoadSite(url);
            
            _logger.LogInformation("Response time: {time} ms", siteCheck.ResponseTimeMs);
            _logger.LogInformation("Response code: {code}", siteCheck.HttpStatusCode);

            CosmosClient client = new CosmosClient(_configuration["CosmosConnectionStrings"]);
            Database db = client.GetDatabase(_configuration["CosmosDb"]);
            Container ct = db.GetContainer(_configuration["CosmosContainer"]);

            SiteHealthItem siteHealthItem = new SiteHealthItem();
            siteHealthItem.ResponseTimeMs = siteCheck.ResponseTimeMs;
            siteHealthItem.HttpStatusCode = siteCheck.HttpStatusCode;

            await ct.CreateItemAsync(siteHealthItem);
        }
    }
}
