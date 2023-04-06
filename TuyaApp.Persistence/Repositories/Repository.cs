using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Domain.Common;
using TuyaApp.Persistence.Context;

namespace TuyaApp.Persistence.Repositories
{
    // This is a generic repository class that implements IRepository<T> interface.
    // It takes a DbContext object in the constructor and exposes CRUD operations for the entity type T.
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        readonly TuyaAppDbContext _context;
        readonly DbSet<T> _dbSet;

        public Repository(TuyaAppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Adds the given entity to the context and returns true if it is added successfully
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await _dbSet.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        //Adds the given entities to the context
        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return true;
        }

        // Gets all entities of type T and returns an IQueryable<T>
        public IQueryable<T> GetAll(bool tracking = true)
        {
           var query = _dbSet.AsQueryable();
            if(!tracking)
                query = query.AsNoTracking();

            return query;
        }
            

        // Gets all entities of type T that satisfy the given expression and returns an IQueryable<T>
        public IQueryable<T> GetAll(Expression<Func<T,bool>> expression, bool tracking = true)
        {
            var query = _dbSet.Where(expression);

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }
             

        // Gets the entity of type T with the given id and returns an IQueryable<T>
        public IQueryable<T> GetById(int id) =>        
             _dbSet.AsQueryable().Where(w => w.Id == id);

        // Removes the given entity from the context and returns true if it is removed successfully
        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = _dbSet.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        // Removes the entity of type T with the given id from the context and returns true if it is removed successfully
        public async Task<bool> RemoveAsync(int id)
        {
            T model = await _dbSet.FirstOrDefaultAsync(data => data.Id == id);
            return Remove(model);
        }

        //Removes more than one entities
        public bool RemoveRange(List<T> datas)
        {
            _dbSet.RemoveRange(datas);
            return true;
        }

        // Saves changes made to the context and returns the number of entities saved
        public async Task<int> SaveAsync() =>
                await _context.SaveChangesAsync();

        // Updates the given entity in the context and returns true if it is updated successfully
        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = _dbSet.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
