using System;
using System.Collections;
using System.Threading;

namespace Cryptonite.Core.Common
{
    public class DateTimeProviderContext : IDisposable
    {
        private static readonly ThreadLocal<Stack> _threadScopeStack = new(() => new Stack());
        internal DateTime ContextDateTimeNow;

        public DateTimeProviderContext(DateTime contextDateTimeNow)
        {
            ContextDateTimeNow = contextDateTimeNow;
            _threadScopeStack.Value.Push(this);
        }

        public static DateTimeProviderContext Current
        {
            get
            {
                if (_threadScopeStack.Value.Count == 0)
                {
                    return null;
                }

                return _threadScopeStack.Value.Peek() as DateTimeProviderContext;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _threadScopeStack.Value.Pop();
        }
    }
}