using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipIt.PriceQuotes.Api.Contracts;

namespace ShipIt.PriceQuotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceQuotesController : ControllerBase
    {
        [HttpPost]
        public ActionResult<PriceQuoteResponseContract> CalculatePrice(
            [FromBody]PriceQuoteRequestContract requestContract)
        {
            var price = requestContract.WeightG * 30;
            return Ok( 
                new PriceQuoteResponseContract
                {
                    Price = price,
                    ValidUntil = DateTime.UtcNow.AddHours(48)
                }
            );
        }
    }
}
