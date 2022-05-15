using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Infrastructure.Commands.TradeEntries.Delete;
using Cryptonite.Infrastructure.Commands.TradeEntries.Update;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.TradeEntries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers;

[Authorize]
[Route("api/v1.0/[controller]")]
public class TradeEntriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserInfo _userInfo;

    public TradeEntriesController(IMediator mediator, IUserInfo userInfo)
    {
        _mediator = mediator;
        _userInfo = userInfo;
    }

    [HttpGet("table-list")]
    public async Task<IActionResult> GetList([FromQuery] PaginatedSearch search)
    {
        return this.Result(await _mediator.Send(new TradeEntryListQuery().WithSearchParameters(search).WithUserId(_userInfo.Id),
            HttpContext.RequestAborted));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTradeEntryCommand command)
    {
        return this.Result(await _mediator.Send(command.WithId(id).WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return this.Result(await _mediator.Send(new DeleteTradeEntryCommand().WithId(id).WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }

    [HttpPost("delete-batch")]
    public async Task<IActionResult> DeleteBatch([FromBody] DeleteBatchTradeEntryCommand command)
    {
        return this.Result(await _mediator.Send(command.WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }
}