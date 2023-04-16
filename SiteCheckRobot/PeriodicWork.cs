using Microsoft.Azure.Cosmos;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SiteCheckRobot;

public class PeriodicWork
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
        // TODO: Move SiteCheck logic into this class and rename this class to SiteCheck
        var siteCheck = new SiteCheck();
        var url = "https://www.matthias-jost.ch";
        await siteCheck.LoadSite(url);

        _logger.LogInformation("Response time: {time} ms", siteCheck.ResponseTimeMs);
        _logger.LogInformation("Response code: {code}", siteCheck.HttpStatusCode);

        CosmosClient client = new CosmosClient(_configuration["CosmosConnectionStrings"]);
        Database db = client.GetDatabase(_configuration["CosmosDb"]);
        Container ct = db.GetContainer(_configuration["CosmosContainer"]);

        SiteHealthItem latestItem = await GetLastSiteHealthItem(ct);

        SiteHealthItem siteHealthItem = new SiteHealthItem
        {
            ResponseTimeMs = siteCheck.ResponseTimeMs,
            HttpStatusCode = siteCheck.HttpStatusCode
        };

        if (latestItem != null && latestItem.State != siteHealthItem.State)
        {

            // TODO: Use Twilio.AspNet.Core to add the Twilio client to DI container and inject Twilio client
            TwilioClient.Init(_configuration["TwilioSid"], _configuration["TwilioAuthToken"]);

            var messageOptions = new CreateMessageOptions(to: new PhoneNumber(_configuration["TwilioTo"]));
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

            _ = await MessageResource.CreateAsync(messageOptions);
        }

        await ct.CreateItemAsync(siteHealthItem);
    }

    private static async Task<SiteHealthItem?> GetLastSiteHealthItem(Container ct)
    {
        var query = "SELECT TOP 1 * FROM c ORDER BY c._ts DESC";
        var iterator = ct.GetItemQueryIterator<SiteHealthItem>(new QueryDefinition(query));
        FeedResponse<SiteHealthItem>? resultItems = await iterator.ReadNextAsync();
        SiteHealthItem? latestItem = resultItems?.FirstOrDefault();
        return latestItem;
    }
}