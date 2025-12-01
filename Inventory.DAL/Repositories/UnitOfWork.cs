using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Inventory.DAL.Contracts;
using Inventory.DAL.Data;
using Inventory.Domains;

namespace Inventory.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction _transaction;
        private readonly ILoggerFactory _loggerFactory;

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;

        }

        //Repository caching inside Unit of Work
        public IGenericRepository<T> GetRepository<T>() where T : BaseTable
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var logger = _loggerFactory.CreateLogger<GenericRepository<T>>();
                var repositoryInstance = new GenericRepository<T>(_context, logger);
                _repositories[type] = repositoryInstance;
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        // يبدأ Transaction
        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        // يحفظ التغييرات ويعمل Commit
        public async Task CommitChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                    await _transaction.CommitAsync();
            }
            catch
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                    await _transaction.DisposeAsync();
            }
        }

        // يرجع كل العمليات لو في Error
        public async Task Rollback()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();

            await _context.DisposeAsync();
        }


    }
}
