using IPLMerch.Infrastructure.Data;
using IPLMerch.Infrastructure.Repositories;
using IPLMerch.Services;
using IPLMerch.Services.CoreServices;
using IPLMerch.Services.LoggerServices;
using Microsoft.EntityFrameworkCore;

namespace IPLMerch.Helpers;

public static class ServiceHelper
{
    /// <summary>
    /// Register the Repositories
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<ICartRepository, CartRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddLoggingDecorators();
    }

    /// <summary>
    /// Register the Services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // Register services
        return services.AddScoped<IProductService, ProductService>()
            .AddScoped<IFranchiseService, FranchiseService>()
            .AddScoped<ICartService, CartService>()
            .AddScoped<IOrderService, OrderService>();
    }

    /// <summary>
    /// Configure the DB context for EF
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IPLShopDbContext>(options =>
            options.UseSqlite(connectionString));
        return services;
    }

    private static IServiceCollection AddLoggingDecorators(this IServiceCollection services)
    {
        services.Decorate<IProductService, ProductLogger>();
        return services;
    }
}