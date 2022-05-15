using System.Linq;
using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Core.Enums;
using Cryptonite.Infrastructure.Commands.BuyEntries.Insert;
using Cryptonite.Infrastructure.Commands.ImportEntries;
using Cryptonite.Infrastructure.Commands.TradeEntries.Insert;
using Cryptonite.Infrastructure.CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers
{
    [Authorize]
    [Route("api/v1.0/entries")]
    public class EntriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserInfo _userInfo;

        public EntriesController(IMediator mediator, IUserInfo userInfo)
        {
            _mediator = mediator;
            _userInfo = userInfo;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> InsertBuyEntry([FromBody] InsertBuyEntryCommand command)
        {
            return this.Result(await _mediator.Send(command.WithUserId(_userInfo.Id), HttpContext.RequestAborted));
        }

        [HttpPost("trade")]
        public async Task<IActionResult> InsertTrade([FromBody] InsertTradeEntryCommand command)
        {
            return this.Result(await _mediator.Send(command.WithUserId(_userInfo.Id), HttpContext.RequestAborted));
        }

        [HttpPost("import/{type}")]
        public async Task<IActionResult> ImportEntries(ImportType type)
        {
            return this.Result(await _mediator.Send(new ImportEntriesCommand(HttpContext.Request.Form.Files.First(), type).WithUserId(_userInfo.Id),
                HttpContext.RequestAborted));
        }
    }
}