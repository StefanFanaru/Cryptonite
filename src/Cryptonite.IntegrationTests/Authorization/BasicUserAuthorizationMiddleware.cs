using System.Security.Claims;
using System.Threading.Tasks;
using Cryptonite.API.Asp.UserInfo;
using Microsoft.AspNetCore.Http;

namespace Cryptonite.IntegrationTests.Authorization
{
    internal class BasicUserAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicUserAuthorizationMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("Bearer");
            identity.AddClaims(new Claim[]
            {
                new(Claims.UserId, TestConstants.UserId),
                new(Claims.Email, "test.user@gmail.com"),
                new(Claims.Role, Roles.BasicUser),
                new(Claims.FirstName, "Test"),
                new(Claims.LastName, "User")
            });

            httpContext.User.AddIdentity(identity);

            await _next.Invoke(httpContext);
        }
    }
}