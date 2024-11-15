using System.Linq.Expressions;

namespace SampleTest.Infrastructure.Repositories.Shared
{
    /// <summary>
    /// Interface for base repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Save async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task SaveAsync(T obj);

        /// <summary>
        /// Save range async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task SaveRangeAsync(IList<T> obj);

        /// <summary>
        /// Update async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task UpdateAsync(T obj);

        /// <summary>
        /// Update range async
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(IList<T> objs);

        /// <summary>
        /// Delete async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Get by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Get by id async and includes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(long id, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Find with predicate async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string OrderColumn, bool OrderDirection);

        /// <summary>
        /// Find with predicate and Incluide async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string OrderColumn, bool OrderDirection, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Find with query async
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindWithQueryAsync(IQueryable<T> query, string OrderColumn, bool OrderDirection);

        /// <summary>
        /// Find with expression first or default async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Execute Dapper Commands
        /// </summary>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> ExecuteDapper(string command, object param);

        /// <summary>
        /// Query Dapper Commands
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetDapper(string command, object param);
    }
}
