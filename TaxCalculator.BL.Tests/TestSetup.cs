using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaxCalculator.BL.Tests
{
    [SetUpFixture]
    public class TestSetup
    {
        private static ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            //var startup = new Startup();
            var services = new ServiceCollection();
            //startup.ConfigureForTests(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            _serviceProvider.Dispose();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
