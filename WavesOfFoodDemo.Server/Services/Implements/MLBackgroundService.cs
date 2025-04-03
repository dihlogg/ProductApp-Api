namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class MLBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MLBackgroundService> _logger;

        public MLBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<MLBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        // execute background service / update clusterId
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mlService = scope.ServiceProvider.GetRequiredService<IMLService>();
                    await mlService.UpdateClustersAsync();
                }

                _logger.LogInformation("Cluster data updated successfully.");
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
