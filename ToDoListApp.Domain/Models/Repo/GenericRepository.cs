using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Domain.Models.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        public GenericRepository(DbContext dbContext)
        {
            this._context = dbContext;
        }
        
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task Add(T entity)
        {
            var res = _context.Set<T>().Add(entity);

            return Task.FromResult(res);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public Task Remove(T entity)
        {
            var res = _context.Set<T>().Remove(entity);

            return Task.FromResult(res);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public Task Update(T entity)
        {
            var res = _context.Set<T>().Update(entity);

            return Task.FromResult(res);
        }
    }
}
