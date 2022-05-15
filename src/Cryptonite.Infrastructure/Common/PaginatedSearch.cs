using Cryptonite.Infrastructure.Data.Common;

namespace Cryptonite.Infrastructure.Common
{
    public class PaginatedSearch
    {
        public string Term { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}