using ShipIt.PriceQuotes.Api.Contracts;
using ShipIt.PriceQuotes.Domain.Services.Interfaces;
using ShipIt.PriceQuotes.Storage.DataModel;

namespace ShipIt.PirceQuotes.Domain.Services;

public interface IPriceQuoteService
{
    Task<PriceQuoteResponseContract> CreateQuote(PriceQuoteRequestContract quoteToCreate);
    Task<PriceQuoteResponseContract> GetById(string id);
}

public class PriceQuoteService(IPriceQuoteRepository repository) : IPriceQuoteService
{
    public async Task<PriceQuoteResponseContract> CreateQuote(PriceQuoteRequestContract quoteToCreate)
    {
        if(quoteToCreate.FromCountry == Shared.Enums.DeliverableCountries.NL && quoteToCreate.WeightG > 10000)
            throw new FromCountryWeightExceededException($"FromCountry ({quoteToCreate.FromCountry}) weight ({quoteToCreate.WeightG/1000})kg exceeds 10kg.");

        var volume = quoteToCreate.LengthCm * quoteToCreate.WidthCm * quoteToCreate.HeightCm;
        var calculatedPrice = 2.5 + volume * 0.0001 + (quoteToCreate.WeightG / 1000 * 1.15);

        if(quoteToCreate.FromCountry != quoteToCreate.ToCountry)
            calculatedPrice += 3;

        // map to datamodel
        var datamodel = new PriceQuoteDataModel
        {
            fromCountry = quoteToCreate.FromCountry ?? throw new ArgumentNullException(nameof(quoteToCreate.FromCountry)),
            toCountry = quoteToCreate.ToCountry ?? throw new ArgumentNullException(nameof(quoteToCreate.FromCountry)),
            heightCm = quoteToCreate.HeightCm,
            lengthCm = quoteToCreate.LengthCm,
            widthCm = quoteToCreate.WidthCm,
            weightKg = quoteToCreate.WeightG / 1000.0,
            price = (decimal) calculatedPrice,
            validUntil = DateTime.UtcNow.AddHours(48),
            createdOn = DateTime.UtcNow,            
        };

        // save to database
        var created = await repository.CreateAsync(datamodel);

        // return response
        return new PriceQuoteResponseContract
        {
            Id = created.id,
            Price = (double) created.price,
            ValidUntil = created.validUntil
        };
    }

    public async Task<PriceQuoteResponseContract> GetById(string id)
    {
        var dbQuote = await repository.GetAsync(id);
        
        return new PriceQuoteResponseContract
        {
            Id = dbQuote.id,
            Price = (double) dbQuote.price,
            ValidUntil = dbQuote.validUntil
        };
    }
}

