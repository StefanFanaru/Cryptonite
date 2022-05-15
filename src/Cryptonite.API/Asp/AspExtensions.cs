using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Cryptonite.API.Asp
{
    public static class AspExtensions
    {
        /// <summary>
        ///     Returns an array of stack traces. Useful for improving JSON readability.
        /// </summary>
        /// <param name="stacktrace">The exception's stacktrace</param>
        /// <returns></returns>
        public static List<string> StackTraceJsonFormatter(string stacktrace)
        {
            var result = new List<string>();
            var split = stacktrace.Replace("\r\n", string.Empty).Split("at ");
            foreach (var trace in split)
            {
                if (!string.IsNullOrWhiteSpace(trace)) // to get rid of the first "at  "
                {
                    result.Add("at " + trace);
                }
            }

            return result;
        }

        public static void AddScopedImplementationsOf<TInterface>(this IServiceCollection services,
            Assembly targetAssembly = null)
        {
            targetAssembly ??= Assembly.GetExecutingAssembly();
            var implementationTypes = targetAssembly
                .GetTypes()
                .Where(x => x.IsPublic && x.IsClass)
                .Where(x => x.GetInterfaces().Contains(typeof(TInterface)));

            if (implementationTypes == null)
            {
                throw new InvalidOperationException($"No implementations found for interface {typeof(TInterface).Name}");
            }

            foreach (var implementationType in implementationTypes)
            {
                services.AddScoped(implementationType);
            }
        }
    }
}