using System;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ShipIt.PirceQuotes.Domain.Services;
using ShipIt.PriceQuotes.Domain.Services.Interfaces;
using ShipIt.PriceQuotes.Storage;

namespace ShipIt.PriceQuotes.Api;

public static class ShipItServiceCollectionExtensions
{
    public static IServiceCollection AddShipItRateLimiters(this IServiceCollection services)
    {
        services.AddRateLimiter( limiter => limiter
            .AddFixedWindowLimiter(policyName: "quote-creation-limiter", options => 
            {
                options.PermitLimit = 3; 
                options.Window = TimeSpan.FromSeconds(10); 
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; 
                options.QueueLimit = 0; 
            }));

        services.AddRateLimiter( limiter => limiter
            .AddFixedWindowLimiter(policyName: "andere-rate-limiter", options => 
            {
                options.PermitLimit = 1; 
                options.Window = TimeSpan.FromSeconds(10); 
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; 
                options.QueueLimit = 0; 
            }));

        return services;
    }

    public static IServiceCollection AddShipItSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Options
        services.Configure<PriceQuoteRepositoryOptions>(
            builder.Configuration.GetSection(
                nameof(PriceQuoteRepositoryOptions) 
            )
        );

        // Controllers
        services.AddControllers().AddJsonOptions(opt => {
            opt.JsonSerializerOptions.Converters.Add( 
                new JsonStringEnumConverter());
        });     

        return services;   
    }

    public static IServiceCollection AddShipItReferences(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IPriceQuoteService, PriceQuoteService>();
        // Repos
        services.AddScoped<IPriceQuoteRepository, PriceQuoteRepository>();
        return services;
    }
}
