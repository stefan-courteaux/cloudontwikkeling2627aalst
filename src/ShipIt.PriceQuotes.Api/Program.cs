using System.Text.Json.Serialization;
using ShipIt.PirceQuotes.Domain.Services;
using ShipIt.PriceQuotes.Domain.Services.Interfaces;
using ShipIt.PriceQuotes.Storage;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<PriceQuoteRepositoryOptions>(
    builder.Configuration.GetSection(
        nameof(PriceQuoteRepositoryOptions) 
    )
);

// Controllers
builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.Converters.Add( 
        new JsonStringEnumConverter());
});

// Services
builder.Services.AddScoped<IPriceQuoteService, PriceQuoteService>();

// Repos
builder.Services.AddScoped<IPriceQuoteRepository, PriceQuoteRepository>();




var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment()) 
    app.UseExceptionHandler("/error-development");
else
    app.UseExceptionHandler("/error");

app.Run();
