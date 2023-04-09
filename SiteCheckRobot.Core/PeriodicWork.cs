using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SiteCheckRobot.Core
{
    public class PeriodicWork : IPeriodicWork
    {
        private readonly ILogger<PeriodicWork> _logger;
        private readonly ISiteHealthRepository _siteHealthRepository;

        public PeriodicWork(ILogger<PeriodicWork> logger, ISiteHealthRepository siteHealthRepository)
        {
            _logger = logger;
            _siteHealthRepository = siteHealthRepository;
        }

        public async Task Execute()
        {
            var siteCheck = new SiteCheck();
            var url = "https://www.matthias-jost.ch";
            await siteCheck.LoadSite(url);

            await _siteHealthRepository.Connect();
            _logger.LogInformation("Response time: {time} ms", siteCheck.ResponseTimeMs);
            _logger.LogInformation("Response code: {code}", siteCheck.HttpStatusCode);
        }
    }
}
