using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DownloadCleanupService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string path;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override async Task StartAsync(CancellationToken cts)
        {
            path = KnownFolders.GetPath(KnownFolder.Downloads);
            await base.StartAsync(cts);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var files = Directory.GetFiles(path).ToList();
                foreach(var f in files)
                {
                    File.Delete(f);
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
