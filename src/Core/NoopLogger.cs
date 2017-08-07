using System;
using Microsoft.Extensions.Logging;

namespace Skarp.HubSpotClient.Core
{
    public class NoopLogger<T> : ILogger<T>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }
    }

    public class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}