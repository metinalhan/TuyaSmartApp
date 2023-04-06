using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TuyaApp.Domain.Common;

namespace TuyaApp.Application.Abstractions.Repositories
{
    // This is an interface definition for a generic repository that can be used to perform CRUD (Create, Read, Update, Delete) operations on entities of type T.
    public interface IRepository<T> where T : BaseEntity
    {
        // Returns all entities of type T as an IQueryable.
        IQueryable<T> GetAll(bool tracking = true);

        // Returns all entities of type T that satisfy the given expression as an IQueryable.
        IQueryable<T> GetAll(Expression<Func<T,bool>> expression, bool tracking = true);

        // Returns the entity of type T with the given id.
        IQueryable<T> GetById(int id);

        // Adds the given entity of type T to the repository and returns a boolean indicating whether the operation was successful.
        Task<bool> AddAsync(T entity);

        //Adds the given entities
        Task<bool> AddRangeAsync(List<T> entities);

        // Removes the given entity of type T from the repository and returns a boolean indicating whether the operation was successful.
        bool Remove(T model);

        //Removes the given entities
        bool RemoveRange(List<T> datas);

        // Removes the entity of type T with the given id from the repository and returns a boolean indicating whether the operation was successful.
        Task<bool> RemoveAsync(int id);

        // Saves changes made to the repository and returns the number of affected rows as an integer.
        Task<int> SaveAsync();

        // Updates the given entity of type T in the repository and returns a boolean indicating whether the operation was successful.
        bool Update(T entity);
    }
}
