using System.Text.Json.Serialization;
using ShipIt.PirceQuotes.Domain.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.Converters.Add( 
        new JsonStringEnumConverter());
});
builder.Services.AddScoped<IPriceQuoteService, PriceQuoteService>();

var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment()) 
    app.UseExceptionHandler("/error-development");
else
    app.UseExceptionHandler("/error");

app.Run();
