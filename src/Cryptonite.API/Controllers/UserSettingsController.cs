using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Infrastructure.Commands.UpdateUserSettings;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.UpdateUserSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Controllers
{
    [Route("api/v1.0/user-settings")]
    public class UserSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserInfo _userInfo;

        public UserSettingsController(IMediator mediator, IUserInfo userInfo)
        {
            _mediator = mediator;
            _userInfo = userInfo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserSettings()
        {
            return this.Result(await _mediator.Send(new UserSettingsQuery().WithUserId(_userInfo.Id),
                HttpContext.RequestAborted));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UpdateUserSettingsCommand command)
        {
            return this.Result(await _mediator.Send(command.WithUserId(_userInfo.Id), HttpContext.RequestAborted));
        }
    }
}