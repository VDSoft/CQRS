using System.Threading.Tasks;
using VDsoft.Cqrs.Command;
using VDsoft.Cqrs.Query;

namespace VDsoft.Cqrs.Services
{
    /// <summary>
    /// Defines an action dispatcher to dispatch a command or query.
    /// </summary>
    public interface IActionDispatcher
    {
        /// <summary>
        /// Executes a command based on the provided arguments asynchronously.
        /// </summary>
        /// <param name="commandArguments">The arguments of the command to dispatch.</param>
        /// <returns>A <see cref="Task"/> for the asynchronous operation.</returns>
        Task ExecuteAsync(IArguments commandArguments);

        /// <summary>
        /// Executes the provided query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the result of the query.</typeparam>
        /// <param name="query">The query to dispatch.</param>
        /// <returns>A <see cref="Task"/> for the asynchronous operation, containing the result of the query.</returns>
        Task<T> ExecuteAsync<T>(IQuery<T> query);
    }
}
