using System.Threading.Tasks;
using VDsoft.Cqrs.Command;
using VDsoft.Cqrs.Query;

namespace VDsoft.Cqrs.DependencyInjectionExtension.Test
{
    internal class TestArgument : IArguments
    {
        public TestArgument(string testValue)
        {
            this.TestValue = testValue;
        }

        public string TestValue { get; }
    }

    internal class TestCommand : ICommand<TestArgument>
    {
        public Task ExecuteAsync(TestArgument arguments)
        {
            return Task.FromResult(arguments);
        }
    }

    internal class TestQuery : IQuery<int>
    {
        public TestQuery(int value)
        {
            this.Value = value;
        }

        public int Value { get; }
    }

    internal class TestQueryHandler : IQueryHandler<TestQuery, int>
    {
        public Task<int> ExecuteAsync(TestQuery query)
        {
            return Task.FromResult(query.Value);
        }
    }
}
