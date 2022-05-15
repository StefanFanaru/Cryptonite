using System.Net;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.CQRS.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Cryptonite.API.Asp
{
    public static class ControllerExtensions
    {
        public static IActionResult Result<T>(this ControllerBase controller, IOperationResult<T> result)
        {
            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return new NoContentResult();
            }

            return new OperationActionResult<T>(result);
        }
    }
}