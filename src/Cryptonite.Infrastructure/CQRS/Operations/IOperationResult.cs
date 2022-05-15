using System.Net;
using Cryptonite.Infrastructure.CQRS.Errors;

namespace Cryptonite.Infrastructure.CQRS.Operations
{
    public interface IOperationResult<out TResult>
    {
        TResult Result { get; }
        bool IsSuccess { get; }
        Error Error { get; }
        public bool HasResult { get; }
        public HttpStatusCode StatusCode { get; set; }
    }
}