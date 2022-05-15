using Cryptonite.Infrastructure.Common;

namespace Cryptonite.Infrastructure.CQRS;

public static class RequestHelpers
{
    public static T WithSearchParameters<T>(this T query, PaginatedSearch searchParameters) where T : IUserBasedSearchQuery
    {
        query.SearchParameters = searchParameters;
        return query;
    }
}