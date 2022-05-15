using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Infrastructure.Commands.BuyEntries.Delete;
using Cryptonite.Infrastructure.Commands.BuyEntries.Update;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.BuyEntries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers;

[Authorize]
[Route("api/v1.0/[controller]")]
public class BuyEntriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserInfo _userInfo;

    public BuyEntriesController(IMediator mediator, IUserInfo userInfo)
    {
        _mediator = mediator;
        _userInfo = userInfo;
    }

    [HttpGet("table-list")]
    public async Task<IActionResult> GetList([FromQuery] PaginatedSearch search)
    {
        return this.Result(await _mediator.Send(new BuyEntryListQuery().WithSearchParameters(search).WithUserId(_userInfo.Id),
            HttpContext.RequestAborted));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBuyEntryCommand command)
    {
        return this.Result(await _mediator.Send(command.WithId(id).WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return this.Result(await _mediator.Send(new DeleteBuyEntryCommand().WithId(id).WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }

    [HttpPost("delete-batch")]
    public async Task<IActionResult> DeleteBatch([FromBody] DeleteBatchBuyEntryCommand command)
    {
        return this.Result(await _mediator.Send(command.WithUserId(_userInfo.Id), HttpContext.RequestAborted));
    }
}