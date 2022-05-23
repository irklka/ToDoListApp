using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ToDoListApp.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        void AddRange(IEnumerable<T> entities);
        Task Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task Update(T entity);
    }
}
