namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class DailyProductBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DailyProductBackgroundService> _logger;

        public DailyProductBackgroundService(IServiceProvider serviceProvider, ILogger<DailyProductBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var nowUtc = DateTime.UtcNow;
                    var nowVn = nowUtc.AddHours(7); // giờ vn

                    var nextRunVn = nowVn.Date.AddDays(1).AddHours(0); // 12 a.m giờ vn
                    var nextRunUtc = nextRunVn.AddHours(-7); // conver UTC to delay 12h a.m tomorrow

                    var delay = nextRunUtc - nowUtc;

                    await Task.Delay(delay, stoppingToken);

                    // tạo scope và chạy logic
                    using var scope = _serviceProvider.CreateScope();
                    var productService = scope.ServiceProvider.GetRequiredService<IProductInfoHistoryService>();
                    await productService.GenerateDailyTopSellingProducts();

                    _logger.LogInformation("Successfully generated top selling products.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating top selling products");
                }
            }
        }
    }
}
