using Microsoft.Extensions.DependencyInjection;
using WavesOfFoodDemo.Server.Infrastructures;
using WavesOfFoodDemo.Server.Services;

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
    }
}
