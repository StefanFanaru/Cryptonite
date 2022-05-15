using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.Portofolio.Distribution;
using Cryptonite.Infrastructure.Queries.Portofolio.List;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.Today;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers
{
    [Authorize]
    [Route("api/v1.0/portofolio")]
    public class PortofolioController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserInfo _userInfo;

        public PortofolioController(IMediator mediator, IUserInfo userInfo)
        {
            _mediator = mediator;
            _userInfo = userInfo;
        }

        [HttpGet("distribution")]
        public async Task<IActionResult> GetCurrenciesNames()
        {
            return this.Result(await _mediator.Send(new DistributionQuery().WithUserId(_userInfo.Id),
                HttpContext.RequestAborted));
        }

        [HttpGet("result/today")]
        public async Task<IActionResult> GetTodayResult()
        {
            return this.Result(await _mediator.Send(new TodayResultQuery().WithUserId(_userInfo.Id), HttpContext.RequestAborted));
        }

        [HttpGet("result/alltime")]
        public async Task<IActionResult> GetAllTimeResult()
        {
            return this.Result(await _mediator.Send(new AllTimeResultQuery().WithUserId(_userInfo.Id), HttpContext.RequestAborted));
        }

        [HttpGet("table-list")]
        public async Task<IActionResult> GetList([FromQuery] PaginatedSearch search)
        {
            return this.Result(await _mediator.Send(new PortofolioCryptocurrenciesListQuery().WithSearchParameters(search).WithUserId(_userInfo.Id),
                HttpContext.RequestAborted));
        }
    }
}