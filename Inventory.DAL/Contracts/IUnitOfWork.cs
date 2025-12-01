using Inventory.Domains;

namespace Inventory.DAL.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {   
        IGenericRepository<T> GetRepository<T>() where T : BaseTable;
        Task BeginTransaction();
        Task CommitChanges();
        Task Rollback();
        Task<int> SaveChangesAsync();
    }


}
