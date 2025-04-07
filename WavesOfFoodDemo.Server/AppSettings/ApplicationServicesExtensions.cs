using WavesOfFoodDemo.Server.Infrastructures;
using WavesOfFoodDemo.Server.Infrastructures.Repositories;
using WavesOfFoodDemo.Server.Services;
using WavesOfFoodDemo.Server.Services.Implements;

namespace WavesOfFoodDemo.Server.AppSettings;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServicesExtension(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IProductInfoRepository, ProductInfoRepository>();
        services.AddTransient<IProductInfoService, ProductInfoService>();
        services.AddTransient<IUserInfoRepository, UserInfoRepository>();
        services.AddTransient<IUserInfoService, UserInfoService>();
        services.AddTransient<ICartInfoRepository, CartInfoRepository>();
        services.AddTransient<ICartInfoService, CartInfoService>();
        services.AddTransient<ICartDetailsRepository, CartDetailsRepository>();
        services.AddTransient<ICartDetailsService, CartDetailsService>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IProductImageRepository, ProductImageRepository>();
        services.AddTransient<IProductImageService, ProductImageService>();
        services.AddTransient<IRedisService, RedisService>();
        services.AddTransient<IMLService, MLService>();
        services.AddTransient<IProductInfoHistoryRepository, ProductInfoHistoryRepository>();
        services.AddTransient<IProductInfoHistoryService, ProductInfoHistoryService>();
    }
}
