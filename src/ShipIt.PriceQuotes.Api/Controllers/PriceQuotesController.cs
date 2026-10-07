
using Microsoft.AspNetCore.Mvc;
using ShipIt.PirceQuotes.Domain.Services;
using ShipIt.PriceQuotes.Api.Contracts;

namespace ShipIt.PriceQuotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceQuotesController(IPriceQuoteService service) : ControllerBase
    {
        [HttpPost]
        public ActionResult<PriceQuoteResponseContract> CalculatePrice(
            [FromBody]PriceQuoteRequestContract requestContract)
        {
            try{
                return Ok(service.CreateQuote(requestContract));
            }
            catch(FromCountryWeightExceededException e)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = nameof(FromCountryWeightExceededException), 
                    Detail = e.Message}
                );
            }
        }
    }
}
