namespace SiteCheckRobot.WebApi
{
    public class SiteCheckBackgroundWorker : IHostedService
    {
        private readonly ILogger<SiteCheckBackgroundWorker> _logger;
        private Timer? _timer;

        public SiteCheckBackgroundWorker(ILogger<SiteCheckBackgroundWorker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                               TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
