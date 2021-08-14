using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDsoft.Cqrs.Services;
using Xunit;

namespace VDsoft.Cqrs.DependencyInjectionExtension.Test
{
    public class MicrosoftDiTests
    {
        [Fact]
        public void UseCqrs_CommandAndArgumentFromTestAssembly_CommandCanBeRequestedFromDi()
        {
            // Arrange
            var services = new ServiceCollection();

            var value = "Test Value";
            var testArguments = new TestArgument(value);

            var dispatcher = new DefaultActionDispatcher(services.BuildServiceProvider());

            // Act
            services.UseCqrs(typeof(TestCommand).Assembly);

            // Assert
            var provider = services.BuildServiceProvider();
            dispatcher.ExecuteAsync(testArguments);
            Assert.NotNull(provider.GetService(typeof(TestArgument)));
        }
    }
}
