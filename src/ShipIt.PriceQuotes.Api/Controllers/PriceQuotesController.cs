
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShipIt.PirceQuotes.Domain.Services;
using ShipIt.PriceQuotes.Api.Contracts;

namespace ShipIt.PriceQuotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceQuotesController(IPriceQuoteService service) : ControllerBase
    {
        [HttpPost]
        [EnableRateLimiting("quote-creation-limiter")]
        [ProducesResponseType(typeof(PriceQuoteResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PriceQuoteResponseContract>> CalculatePrice(
            [FromBody]PriceQuoteRequestContract requestContract)
        {
            try{
                var created = await service.CreateQuote(requestContract);
                return CreatedAtAction(nameof(Get), new { id = created.Id}, created);
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

        [HttpGet("{id}")]
        [EnableRateLimiting("andere-rate-limiter")]
        public async Task<ActionResult<PriceQuoteResponseContract>> Get(
            [FromRoute] string id)
        {
            var quote = await service.GetById(id);
            return quote;
        }        
    }
}
