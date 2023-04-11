using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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

            SiteHealthItem latestItem = await GetLastSiteHealthItem(ct);

            SiteHealthItem siteHealthItem = new SiteHealthItem();
            siteHealthItem.ResponseTimeMs = siteCheck.ResponseTimeMs;
            siteHealthItem.HttpStatusCode = siteCheck.HttpStatusCode;

            if (latestItem != null && latestItem.State != siteHealthItem.State)
            {

                TwilioClient.Init(_configuration["TwilioSid"], _configuration["TwilioAuthToken"]);

                var messageOptions = new CreateMessageOptions(new PhoneNumber(_configuration["TwilioTo"]));
                messageOptions.From = new PhoneNumber(_configuration["TwilioFrom"]);

                if (latestItem.State == "good" && siteHealthItem.State == "bad")
                {
                    // state changed from good to bad, do something

                    messageOptions.Body = "Good -> Bad";

                }
                else if (latestItem.State == "bad" && siteHealthItem.State == "good")
                {
                    // state changed from bad to good, do something else
                    messageOptions.Body = "Bad -> Good";
                }

                var message = await MessageResource.CreateAsync(messageOptions);
            }

            await ct.CreateItemAsync(siteHealthItem);
        }

        private static async Task<SiteHealthItem?> GetLastSiteHealthItem(Container ct)
        {
            var query = $"SELECT TOP 1 * FROM c ORDER BY c._ts DESC";
            var iterator = ct.GetItemQueryIterator<SiteHealthItem>(new QueryDefinition(query));
            FeedResponse<SiteHealthItem>? resultItems = await iterator.ReadNextAsync();
            SiteHealthItem? latestItem = resultItems?.FirstOrDefault();
            return latestItem;
        }
    }
}

