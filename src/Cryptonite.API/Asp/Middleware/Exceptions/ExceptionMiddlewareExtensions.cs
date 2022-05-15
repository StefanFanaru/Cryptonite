using Microsoft.AspNetCore.Builder;

namespace Cryptonite.API.Asp.Middleware.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app, bool isProduction)
        {
            if (isProduction)
            {
                app.UseMiddleware<ExceptionMiddlewareDevelopment>();
                return;
            }

            app.UseMiddleware<ExceptionMiddlewareProduction>();
        }
    }
}