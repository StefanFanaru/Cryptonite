using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Data.Common
{
    // Partial Source: https://gist.github.com/jacobsimeon/3094933
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Uses .Skip() and .Take() to get a PageOf&lt;T&gt;
        /// </summary>
        /// <param name="queryable">Executes the query to get a page and also the total count.</param>
        /// <param name="pageIndex">1-based page number</param>
        /// <param name="itemsPerPage">Number of items on each page</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PageOf<T>> GetPageAsync<T>(this IQueryable<T> queryable, int pageIndex, int itemsPerPage,
            CancellationToken cancellationToken = default)
        {
            var itemsToSkip = pageIndex * itemsPerPage;

            var totalItems = queryable.Count();
            var pageData = await queryable.Skip(itemsToSkip).Take(itemsPerPage).ToListAsync(cancellationToken);

            var page = new PageOf<T>(pageData, pageIndex, itemsPerPage, totalItems);

            return page;
        }

        public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, string property, SortDirection sortDirection)
        {
            return ApplyOrder(source, property, sortDirection == SortDirection.Asc ? "OrderBy" : "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenOrderBy<T>(this IQueryable<T> source, string property, SortDirection sortDirection)
        {
            return ApplyOrder(source, property, sortDirection == SortDirection.Asc ? "ThenBy" : "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>
            (this IQueryable<T> queryable, string propertyName, string sortMethodName)
        {
            //build an expression tree that can be passed as lambda to IQueryable#OrderBy
            var type = typeof(T);
            var paramExpression = Expression.Parameter(type, "parameterExpression");

            var property = type.GetProperty(propertyName);
            var propertyExpression = Expression.Property(paramExpression, property);

            var lambdaType = typeof(Func<,>).MakeGenericType(type, property.PropertyType);
            var lambdaExpression = Expression.Lambda(lambdaType, propertyExpression, paramExpression);

            // dynamically generate a method with the correct type parameters
            var queryableType = typeof(Queryable);
            var orderByMethod = queryableType.GetMethods()
                .Single(m => m.Name == sortMethodName &&
                             m.IsGenericMethodDefinition
                             && m.GetGenericArguments().Length == 2
                             && m.GetParameters().Length == 2)
                .MakeGenericMethod(type, property.PropertyType);

            var result = orderByMethod.Invoke(null, new object[] { queryable, lambdaExpression });
            return (IOrderedQueryable<T>)result;
        }
    }
}