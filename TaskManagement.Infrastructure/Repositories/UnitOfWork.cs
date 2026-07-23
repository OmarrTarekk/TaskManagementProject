using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TaskManagement.Domain.Contracts;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementDbContext _context;

        private readonly Hashtable _repositories = new();

        public UnitOfWork(TaskManagementDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);

                _repositories.Add(type, repository);
            }

            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
