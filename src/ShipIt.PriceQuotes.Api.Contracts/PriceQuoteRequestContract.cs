using System;
using System.ComponentModel.DataAnnotations;

namespace ShipIt.PriceQuotes.Api.Contracts;

public class PriceQuoteRequestContract
{
    public int WidthCm { get; set; }
    //...
    [Range(0,12000)]
    public required int WeightG {get; set;}
}
