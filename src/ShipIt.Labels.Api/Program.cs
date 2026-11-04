using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShipIt.Labels.Domain;
using ShipIt.Labels.Domain.DTO;
using ShipIt.Labels.Storage;
using ShipIt.PriceQuotes.Api.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<IDomainController, DomainController>();

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var app = builder.Build();

app.MapGet("/", () => "Hello");

app.MapPost("api/labels", async Task<CreatedAtRoute<string>> (
    [FromServices] IHttpClientFactory clientFactory,
    [FromServices] IDomainController domain,
        [FromBody] LabelRequestContract contract) => {

    // get price
    var httpClient = clientFactory.CreateClient();
    var baseUrl = "https://stefanc-shipit-aalst-quote.azurewebsites.net";
    var url = $"{baseUrl}/api/pricequotes/{contract.QuoteId}";

    var options = new JsonSerializerOptions();
    options.Converters.Add(new JsonStringEnumConverter());
    options.PropertyNameCaseInsensitive = true;
    var response = await httpClient
        .GetFromJsonAsync<PriceQuoteResponseContract>(url, options); 

    // create label
    var creation = new LabelCreationDTO{
        RecipientAddressLine1 = contract.RecipientAddressLine1,
        RecipientAddressLine2 = contract.RecipientAddressLine2,
        RecipientCountry = contract.RecipientCountry,
        RecipientName = contract.RecipientName,
        ShipmentPrice = response.Price 
    };
    var trackingNumber = await domain.CreateLabelAsync(creation); 

    // return trackingNumber / resource identifier
    return TypedResults.CreatedAtRoute( 
        trackingNumber, "GetLabel", new { trackingnumber = trackingNumber});

}).WithName("Create");

app.MapGet("api/labels/{trackingnumber}",
async Task<Results<Ok<string>, BadRequest, FileContentHttpResult>>(
    [FromRoute] string trackingNumber,
    [FromHeader(Name = "Accept")] string acceptValue,
    IDomainController domain) =>
{
    if(acceptValue == "text/json")
    {
        // Dummy implementatie
        return TypedResults.Ok(trackingNumber);
    }
    else if (acceptValue == "application/pdf")
    {
        var pdf = await domain.GetLabelAsync(trackingNumber);
        return TypedResults.Bytes(
            pdf,
            "application/pdf",
            $"{trackingNumber}.pdf");
    }

    return TypedResults.BadRequest();

}).WithName("GetLabel");

app.Run();

public record LabelRequestContract
{
    public required string QuoteId { get; init; }
    public required string RecipientName { get; init; }
    public required string RecipientAddressLine1 { get; init; }
    public required string RecipientAddressLine2 { get; init; }
    public required string RecipientCountry { get; init; }
}
