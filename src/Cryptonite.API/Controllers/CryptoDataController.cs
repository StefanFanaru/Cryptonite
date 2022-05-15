using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.Infrastructure.Queries.CryptoData.Symbols;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers
{
    [Authorize]
    [Route("api/v1.0/crypto-data")]
    public class CryptoDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CryptoDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("symbols")]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> GetSymbols()
        {
            return this.Result(await _mediator.Send(new SymbolsQuery(), HttpContext.RequestAborted));
        }
    }
}