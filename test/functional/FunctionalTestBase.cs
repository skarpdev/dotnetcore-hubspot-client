using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using RapidCore.Xunit.Logging;
using Xunit.Abstractions;

namespace Skarp.HubSpotClient.FunctionalTests
{
    public abstract class FunctionalTestBase<T>
    {
        protected readonly ITestOutputHelper Output;
        protected readonly LoggerFactory LoggerFactory;
        protected readonly ILogger<T> Logger;

        protected FunctionalTestBase(ITestOutputHelper output)
        {
            Output = output;
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddXunitOutput(output);

            Logger = LoggerFactory.CreateLogger<T>();
        }
    }
}
