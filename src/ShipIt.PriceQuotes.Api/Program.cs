using Scalar.AspNetCore;
using ShipIt.PriceQuotes.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShipItSettings(builder);
builder.Services.AddShipItRateLimiters();
builder.Services.AddShipItReferences();
builder.Services.AddOpenApi(); 

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment()) 
    app.UseExceptionHandler("/error-development");
else
    app.UseExceptionHandler("/error");

app.UseRateLimiter();
app.MapOpenApi(); 
app.MapScalarApiReference();

app.Run();
