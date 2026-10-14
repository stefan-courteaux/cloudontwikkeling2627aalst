using System;
using ShipIt.PriceQuotes.Storage.DataModel;

namespace ShipIt.PriceQuotes.Domain.Services.Interfaces;

public interface IPriceQuoteRepository
{
    Task<PriceQuoteDataModel> CreateAsync(PriceQuoteDataModel dataModel);
    Task<PriceQuoteDataModel> GetAsync(string Id);
}
