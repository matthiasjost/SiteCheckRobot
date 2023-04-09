using SiteCheckRobot.Core;

namespace SiteCheckRobot.WebApi
{
   // create background service class
   public class SiteCheckBackgroundWorker : BackgroundService
   {
       private readonly ILogger<SiteCheckBackgroundWorker> _logger;

        public SiteCheckBackgroundWorker(ILogger<SiteCheckBackgroundWorker> logger)
        {
            _logger = logger;
        }

       protected override async Task ExecuteAsync(CancellationToken stoppingToken)
       {
           while (!stoppingToken.IsCancellationRequested)
           {
               _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
               // do work here
               var siteCheck = new SiteCheck();
               var url = "https://www.matthias-jost.ch";
               await siteCheck.LoadSite(url);
               _logger.LogInformation("Response time: {time} ms", siteCheck.ResponseTimeMs);
               _logger.LogInformation("Response code: {code}", siteCheck.HttpStatusCode);
               await Task.Delay(10000, stoppingToken);
           }
       }
   }
}
