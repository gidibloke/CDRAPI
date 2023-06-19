using CDRAPI.DTOs;
using CDRAPI.Interfaces;

namespace CDRAPI.Services
{
    public class BackgroundWorker : BackgroundService
    {
        //Implementing a background service for simplicity. For enterprise application, I would use Hangfire as enterprise applications usually need Cron scheduled services
        private readonly IBackgroundQueue<RequestInformation> _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundWorker> _logger;

        public BackgroundWorker(IBackgroundQueue<RequestInformation> queue, IServiceScopeFactory scopeFactory, ILogger<BackgroundWorker> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{BackgroundWorker} is not running in the background", typeof(BackgroundWorker));
            await BackgroundProcessing(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical(
                "The {BackgroundWorker} is stopping due to a host shutdown, queued items might not be processed anymore.", nameof(BackgroundWorker));

            return base.StopAsync(cancellationToken);
        }


        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(5000, stoppingToken);
                    var request = _queue.Dequeue();
                    if (request == null) continue;
                    _logger.LogInformation("Request found. Starting the upload process..");
                    using var scope = _scopeFactory.CreateScope();
                    var uploadRecords = scope.ServiceProvider.GetRequiredService<IUploadCallRecords>();
                    await uploadRecords.UploadRecords();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error upload call records", ex);
                }
            }
        }
    }
}
