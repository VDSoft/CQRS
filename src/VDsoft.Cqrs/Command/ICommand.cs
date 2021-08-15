using System.Threading.Tasks;

namespace VDsoft.Cqrs.Command
{
    /// <summary>
    /// Defines a command to be used in CQRS.
    /// </summary>
    /// <typeparam name="TArguments">The type of the arguments.</typeparam>
    public interface ICommand<TArguments>
        where TArguments : IArguments
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="arguments">The arguments needed for the command.</param>
        /// <returns>
        /// A <see cref="Task" /> for the asynchronous operation.
        /// </returns>
        Task ExecuteAsync(TArguments arguments);
    }
}
