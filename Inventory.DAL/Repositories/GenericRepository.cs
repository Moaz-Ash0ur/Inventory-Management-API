using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Inventory.DAL.Contracts;
using Inventory.DAL.Data;
using Inventory.DAL.Helper;
using Inventory.DAL.Models;
using Inventory.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseTable
    {

        private readonly AppDbContext _Context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<IGenericRepository<T>> _logger;

        public GenericRepository(AppDbContext context, ILogger<IGenericRepository<T>> logger)
        {
            _Context = context;
            _dbSet = _Context.Set<T>();
            _logger = logger;
        }

        public bool ChangeStatus(int ID, string userId="")
        {

            var entity = GetByID(ID);

            if (entity != null)
            {
                entity.CurrentState = 1;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = userId;
                _Context.Entry(entity).State = EntityState.Modified;
                Save();
                return true;
            }
            return false;
        }

        public bool Delete(int ID)
        {

            try
            {
                var entity = GetByID(ID);

                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

            return false;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.Where(e => e.CurrentState != 1).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public IQueryable<T> GetAllQueryable()
        {
            try
            {
                return _dbSet.Where(e => e.CurrentState != 1);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public T GetByID(int Id)
        {
            try
            {
                var entity = _dbSet.AsNoTracking().SingleOrDefault(e => e.Id == Id);
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public bool Insert(T entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                _dbSet.Add(entity);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public bool Update(T entity)
        {
            try
            {
                var currentEntity = GetByID(entity.Id);

                entity.UpdatedDate = DateTime.Now;
                entity.CreatedDate = currentEntity.CreatedDate;
                entity.CreatedBy = currentEntity.CreatedBy;

                _Context.Entry(entity).State = EntityState.Modified;
                Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public T GetFirst(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().FirstOrDefault();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public List<T> GetList(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().ToList();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public bool Insert(T entity, out int Id)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = "123Moaz";
                _dbSet.Add(entity);
                Save();
                Id = entity.Id;
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }
    }
}
