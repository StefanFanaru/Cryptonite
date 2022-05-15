using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.CQRS
{
    public class UserBasedQuery<T> : IRequest<IOperationResult<T>>, IIncludesUserId
    {
        public string UserId { get; set; }
    }

    public class UserBasedIdQuery<T> : UserBasedQuery<T>, IIncludesId
    {
        public string Id { get; set; }
    }

    public class UserBasedCommand<T> : IRequest<IOperationResult<T>>, IIncludesUserId
    {
        public string UserId { get; set; }
    }

    public class UserBasedIdCommand<T> : UserBasedCommand<T>, IIncludesId
    {
        public string Id { get; set; }
    }

    public interface IIncludesUserId
    {
        public string UserId { get; set; }
    }

    public interface IIncludesId
    {
        public string Id { get; set; }
    }

    public static class UserBasedHelpers
    {
        public static T WithUserId<T>(this T query, string userId) where T : IIncludesUserId
        {
            query.UserId = userId;
            return query;
        }

        public static T WithId<T>(this T query, string id) where T : IIncludesId
        {
            query.Id = id;
            return query;
        }
    }
}