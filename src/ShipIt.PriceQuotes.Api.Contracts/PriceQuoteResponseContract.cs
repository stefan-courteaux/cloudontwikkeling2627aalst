namespace ShipIt.PriceQuotes.Api.Contracts;

public class PriceQuoteResponseContract
{
    public string Id { get; set; }
    public double Price { get; set; }
    public DateTime ValidUntil { get; set; }
}
