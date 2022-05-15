using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cryptonite.IntegrationTests.Authorization
{
    public class RuntimeMiddlewareService
    {
        private IApplicationBuilder _appBuilder;
        private Func<RequestDelegate, RequestDelegate> _middleware;

        public void Use(IApplicationBuilder app)
        {
            _appBuilder = app.Use(next => context => _middleware == null ? next(context) : _middleware(next)(context));
        }

        public void Configure(Action<IApplicationBuilder> action)
        {
            var app = _appBuilder.New();
            action(app);
            _middleware = next => app.Use(_ => next).Build();
        }
    }

    public static class RuntimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseRuntimeMiddleware(this IApplicationBuilder app,
            Action<IApplicationBuilder> defaultAction = null)
        {
            var service = app.ApplicationServices.GetRequiredService<RuntimeMiddlewareService>();
            service.Use(app);
            if (defaultAction != null)
            {
                service.Configure(defaultAction);
            }

            return app;
        }

        public static void SwitchToBasicUser(this RuntimeMiddlewareService service)
        {
            service.Configure(app => app.UseMiddleware<BasicUserAuthorizationMiddleware>());
        }

        public static void SwitchToAdmin(this RuntimeMiddlewareService service)
        {
            service.Configure(app => app.UseMiddleware<AdminAuthorizationMiddleware>());
        }
    }
}