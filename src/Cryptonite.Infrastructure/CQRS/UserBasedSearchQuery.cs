using Cryptonite.Infrastructure.Common;

namespace Cryptonite.Infrastructure.CQRS;

public class UserBasedSearchQuery<T> : UserBasedQuery<T>, IUserBasedSearchQuery
{
    public PaginatedSearch SearchParameters { get; set; }
}

public interface IUserBasedSearchQuery
{
    public PaginatedSearch SearchParameters { get; set; }
}