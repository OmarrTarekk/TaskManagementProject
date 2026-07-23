using System.Linq.Expressions;

namespace TaskManagement.Domain.Contracts
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(
            int pageIndex,
            int pageSize,
            CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task AddAsync(
            TEntity entity,
            CancellationToken ct = default);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
    int page,
    int limit,
    CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> GetAllIncludingAsync(
    CancellationToken ct = default,
    params Expression<Func<TEntity, object>>[] includes);

    }
}