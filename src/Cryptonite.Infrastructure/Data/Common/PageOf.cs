using System.Collections.Generic;

namespace Cryptonite.Infrastructure.Data.Common
{
    public class PageOf<T>
    {
        public PageOf(List<T> pageData, int currentPage, int itemsPerPage, int totalItems)
        {
            PageData = pageData;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
        }

        public List<T> PageData { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }
}