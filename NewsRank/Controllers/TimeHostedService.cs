// ================================
// Name: TimeHostedService
// Description: 定时后台任务服务;
//                     在DoWork方法中部署爬虫;
// Author: Muhz
// Create Date: 2019-07-07
// ================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NewsRank.Controllers
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        // Get configuration from appsetting.json.
        private readonly IConfiguration configuration = new ConfigurationBuilder()    
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(configuration.GetValue<int>("Interval")));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            NewsSpider newsSpider = new NewsSpider(configuration);    // Init Spider.
            newsSpider.Run();    // execute spider.

            _logger.LogInformation("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
