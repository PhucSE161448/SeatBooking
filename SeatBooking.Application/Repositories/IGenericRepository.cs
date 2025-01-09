using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace SeatBooking.Application.Repositories
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        Task<int> GetIdByAccountIdAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!);

        Task<TResult> GetAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T?, bool>> predicate = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!);

        Task<ICollection<T?>> GetListAsync(
            Expression<Func<T?, bool>> predicate = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!);

        Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T?, bool>> predicate = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!);


        #region Insert

        Task<T?> InsertAsync(T? entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        #endregion

        #region Update

        void UpdateAsync(T? entity);

        void UpdateRange(IEnumerable<T?> entities);

        #endregion

        void DeleteAsync(T? entity);
        void DeleteRangeAsync(IEnumerable<T?> entities);
        void Detach(DbContext context, object entity);
    }
}
