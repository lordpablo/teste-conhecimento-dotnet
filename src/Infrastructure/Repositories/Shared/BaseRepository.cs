using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using SampleTest.Domain.Interfaces;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace SampleTest.Infrastructure.Repositories.Shared
{
    /// <summary>
    /// BaseRepository generic class implementations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Context variable
        /// </summary>
        public readonly DbContext _context;

        /// <summary>
        /// dbSet generic
        /// </summary>
        public readonly DbSet<T> _dbSet;

        /// <summary>
        /// Configuration properties
        /// </summary>
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// BaseRepository constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        protected BaseRepository(DbContext context, IConfiguration configuration)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _configuration = configuration;
        }

        #region EntityFramework

        /// <summary>
        /// Find with expression async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="OrderDirection"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string OrderColumn, bool OrderDirection)
        {
            IQueryable<T> query =
                _dbSet.AsQueryable().AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrWhiteSpace(OrderColumn))
                query = query.OrderByDynamic(OrderColumn, OrderDirection);

            var objReturn = await query.AsNoTracking().ToListAsync();
            UncoupleAllEntitys();
            return objReturn;
        }

        /// <summary>
        /// Find with expression and Incluide async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="OrderDirection"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string OrderColumn, bool OrderDirection, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(OrderColumn))
                query = query.OrderByDynamic(OrderColumn, OrderDirection);

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }
            if (predicate != null)
            {
                query = query.AsNoTracking().Where(predicate);
            }

            var objReturn = await query.AsNoTracking().ToListAsync();
            UncoupleAllEntitys();
            return objReturn;
        }

        /// <summary>
        /// Find with expression first or default async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            var objReturn = await query.AsNoTracking().FirstOrDefaultAsync(predicate);
            UncoupleAllEntitys();
            return objReturn;
        }

        /// <summary>
        /// Find with query async
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindWithQueryAsync(IQueryable<T> query, string OrderColumn, bool OrderDirection)
        {
            if (!string.IsNullOrWhiteSpace(OrderColumn))
                query = query.OrderByDynamic(OrderColumn, OrderDirection);

            var objReturn = await query.AsNoTracking().ToListAsync();
            UncoupleAllEntitys();
            return objReturn;
        }

        /// <summary>
        /// Get by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var returnObj = await _dbSet.FindAsync(id);
            UncoupleAllEntitys();
            return returnObj;
        }

        /// <summary>
        /// Get by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(long id, params Expression<Func<T, object>>[] includeProperties)
        {
            if (includeProperties != null)
            {
                UncoupleAllEntitys();
                IQueryable<T> query = _dbSet;

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

                return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
            }
            else
            {
                var returnObj = await _dbSet.FindAsync(id);
                UncoupleAllEntitys();
                return returnObj;
            }
        }

        /// <summary>
        /// Save async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task SaveAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Save range async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task SaveRangeAsync(IList<T> obj)
        {
            await _dbSet.AddRangeAsync(obj);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Update async
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(T obj)
        {
            _dbSet.Update(obj);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Update range async
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public virtual async Task UpdateRangeAsync(IList<T> objs)
        {
            _dbSet.UpdateRange(objs);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Delete async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(int id)
        {
            _dbSet.Remove(await _dbSet.FindAsync(id));
            await SaveChangesAsync();
        }

        #region Private Methods
        /// <summary>
        /// This Method uncouple all entitys
        /// </summary>
        private void UncoupleAllEntitys()
        {
            var tracking = _context.ChangeTracker.Entries().ToList();

            foreach (var entry in tracking)
                entry.State = EntityState.Detached;
        }

        /// <summary>
        /// Save changes async
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> SaveChangesAsync()
            => (await _context.SaveChangesAsync()) > 0;
        #endregion

        #endregion

        #region Dapper
        public async Task<IEnumerable<T>> GetDapper(string command, object param)
        {
            var connection = _context.Database.GetDbConnection();

            if (_context.Database.CurrentTransaction == null)
                return await connection.QueryAsync<T>(command, param);
            else
            {
                var dbTransaction = _context.Database.CurrentTransaction.GetDbTransaction();
                return await connection.QueryAsync<T>(command, param, dbTransaction);
            }
        }
        public async Task<bool> ExecuteDapper(string command, object param)
        {
            var connection = _context.Database.GetDbConnection();
            int count;
            if (_context.Database.CurrentTransaction == null)
                count = await connection.ExecuteAsync(command, param);
            else
            {
                var dbTransaction = _context.Database.CurrentTransaction.GetDbTransaction();
                count = await connection.ExecuteAsync(command, param, dbTransaction);
            }
            return count >= 0;
        }
        #endregion
    }
}
