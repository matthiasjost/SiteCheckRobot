using SiteCheckRobot.Core;

namespace SiteCheckRobot.WebApi
{
    // create background service class
    public class SiteCheckBackgroundWorker : BackgroundService
    {
        private readonly ILogger<SiteCheckBackgroundWorker> _logger;
        private readonly IPeriodicWork _periodicWork;

        public SiteCheckBackgroundWorker(ILogger<SiteCheckBackgroundWorker> logger, IPeriodicWork periodicWork)
        {
            _logger = logger;
            _periodicWork = periodicWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DateTime lastTimeExecuted = DateTime.Now;

            await _periodicWork.Execute();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                if (DateTime.Now.Subtract(lastTimeExecuted).TotalMinutes > 1)
                {
                    await _periodicWork.Execute();

                    lastTimeExecuted = DateTime.Now;
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
