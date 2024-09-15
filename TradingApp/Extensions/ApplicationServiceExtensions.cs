using Microsoft.EntityFrameworkCore;
using TradingApp.Contexts;

namespace TradingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            }, ServiceLifetime.Scoped);

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            //Scopable Contexts
            services.AddHttpClient<AlpacaMDContext>(client =>
            {
                client.BaseAddress = new Uri("https://data.alpaca.markets/v2/stocks/bars");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("APCA-API-KEY-ID", config["Alpaca:Key"]);
                client.DefaultRequestHeaders.Add("APCA-API-SECRET-KEY", config["Alpaca:Secret"]);
            });

            return services;
        }        
    }
}