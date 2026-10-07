using System;
using System.ComponentModel.DataAnnotations;
using ShipIt.Shared.Enums;

namespace ShipIt.PriceQuotes.Api.Contracts;

public class PriceQuoteRequestContract
{
    [Range(1,100)]
    public int WidthCm { get; set; }
    [Range(1,100)]
    public int LengthCm { get; set; }
    [Range(1,100)]
    public int HeightCm { get; set; }
    [Range(1,25000)]
    public required int WeightG {get; set;}
    [Required]
    public DeliverableCountries? FromCountry { get; set; }
    [Required]
    public DeliverableCountries? ToCountry { get; set; }
}
