using ShipIt.PriceQuotes.Api.Contracts;

namespace ShipIt.PirceQuotes.Domain.Services;

public interface IPriceQuoteService
{
    PriceQuoteResponseContract CreateQuote(PriceQuoteRequestContract quoteToCreate);
}

public class PriceQuoteService : IPriceQuoteService
{
    public PriceQuoteResponseContract CreateQuote(PriceQuoteRequestContract quoteToCreate)
    {
        if(quoteToCreate.FromCountry == Shared.Enums.DeliverableCountries.NL && quoteToCreate.WeightG > 10000)
            throw new FromCountryWeightExceededException($"FromCountry ({quoteToCreate.FromCountry}) weight ({quoteToCreate.WeightG/1000})kg exceeds 10kg.");

        var volume = quoteToCreate.LengthCm * quoteToCreate.WidthCm * quoteToCreate.HeightCm;
        var price = 2.5 + volume * 0.0001 + (quoteToCreate.WeightG / 1000 * 1.15);

        if(quoteToCreate.FromCountry != quoteToCreate.ToCountry)
            price += 3;

        return new PriceQuoteResponseContract
        {
            Price = Math.Round(price,1),
            ValidUntil = DateTime.UtcNow.AddHours(48)
        };
    }
}

