using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Contracts;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly TaskManagementDbContext _context;

        public GenericRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(
            int pageIndex,
            int pageSize,
            CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .FindAsync(new object[] { id }, ct);
        }

        public async Task AddAsync(
            TEntity entity,
            CancellationToken ct = default)
        {
            await _context.Set<TEntity>()
                .AddAsync(entity, ct);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>()
                .Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>()
                .Remove(entity);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .AnyAsync(predicate, ct);
        }

        public async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
    int page,
    int limit,
    CancellationToken ct = default)
        {
            var totalCount = await _context.Set<TEntity>().CountAsync(ct);

            var items = await _context.Set<TEntity>()
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllIncludingAsync(
    CancellationToken ct = default,
    params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync(ct);
        }
    }
}