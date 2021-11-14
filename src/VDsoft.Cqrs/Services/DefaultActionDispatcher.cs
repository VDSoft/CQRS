using System;
using System.Threading.Tasks;
using VDsoft.Cqrs.Command;
using VDsoft.Cqrs.Query;

namespace VDsoft.Cqrs.Services
{
    /// <summary>
    /// Default implementation of a <see cref="IActionDispatcher"/>-
    /// </summary>
    /// <seealso cref="IActionDispatcher" />
    public class DefaultActionDispatcher : IActionDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultActionDispatcher"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DefaultActionDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes a command based on the provided arguments asynchronously.
        /// </summary>
        /// <param name="commandArguments">The arguments of the command to dispatch.</param>
        /// <returns>
        /// A <see cref="Task" /> for the asynchronous operation.
        /// </returns>
        public Task ExecuteAsync(IArguments commandArguments)
        {
            var type = typeof(ICommand<>);
            Type[] typeArgs = { commandArguments.GetType() };
            var commandType = type.MakeGenericType(typeArgs);

            dynamic command = serviceProvider.GetService(commandType) ?? 
                throw new Exception($"No command which handles arguments of type {commandArguments.GetType()} could be found.\nMake sure you have a command which accepts arguments of type {commandArguments.GetType()}");

            return command.ExecuteAsync((dynamic)commandArguments);
        }

        /// <summary>
        /// Executes the provided query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the result of the query.</typeparam>
        /// <param name="query">The query to dispatch.</param>
        /// <returns>
        /// A <see cref="Task" /> for the asynchronous operation, containing the result of the query.
        /// </returns>
        public Task<T> ExecuteAsync<T>(IQuery<T> query)
        {
            var type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            var queryCommandType = type.MakeGenericType(typeArgs);

            dynamic queryCommand = serviceProvider.GetService(queryCommandType) ??
                throw new Exception($"No handler for the query {query.GetType()} could be found.\nMake sure you have a handler which accepts query of the type {query.GetType()}."); ;

            return queryCommand.ExecuteAsync((dynamic)query);
        }
    }
}
