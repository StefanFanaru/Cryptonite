using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.Infrastructure.Queries.Currencies.CurrenciesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers
{
    [Authorize]
    [Route("api/v1.0/currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrenciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("names")]
        [ResponseCache(Duration = 604800)]
        public async Task<IActionResult> GetCurrenciesNames()
        {
            return this.Result(await _mediator.Send(new CurrenciesListQuery(), HttpContext.RequestAborted));
        }
    }
}