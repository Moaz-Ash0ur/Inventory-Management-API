using Inventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQueryable();
        T GetByID(int Id);
        bool Insert(T entity);
        bool Insert(T entity,out int Id);
        bool Delete(int ID);
        bool ChangeStatus(int ID, string userID="");
        bool Update(T entity);
        void Save();
        T GetFirst(Expression<Func<T, bool>> filterExpression);
        List<T> GetList(Expression<Func<T, bool>> filterExpression);

    }


}
