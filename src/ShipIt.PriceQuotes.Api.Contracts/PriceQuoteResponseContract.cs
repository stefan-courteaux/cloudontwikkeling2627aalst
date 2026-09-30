namespace ShipIt.PriceQuotes.Api.Contracts;

public class PriceQuoteResponseContract
{
    public double Price { get; set; }
    public DateTime ValidUntil { get; set; }
}
