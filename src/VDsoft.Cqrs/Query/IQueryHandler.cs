using System.Threading.Tasks;

namespace VDsoft.Cqrs.Query
{
    /// <summary>
    /// Defines a handler for a query.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Executes the query asynchronously.
        /// </summary>
        /// <param name="query">The query to send.</param>
        /// <returns>
        /// A <see cref="Task" /> for the asynchronous operation, containing the result of the query.
        /// </returns>
        Task<TResult> ExecuteAsync(TQuery query);
    }
}
