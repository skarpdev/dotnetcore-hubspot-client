using Microsoft.Extensions.Logging;
using RapidCore.Xunit.Logging;
using Xunit.Abstractions;

namespace integration
{
    public abstract class IntegrationTestBase<T>
    {
        protected readonly ITestOutputHelper Output;
        protected readonly LoggerFactory LoggerFactory;
        protected readonly ILogger<T> Logger;

        protected IntegrationTestBase(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddXunitOutput(output);

            Logger = LoggerFactory.CreateLogger<T>();
        }
    }
}