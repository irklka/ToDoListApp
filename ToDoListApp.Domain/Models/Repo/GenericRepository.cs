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
        protected readonly ToDoListDbContext _context;
        public GenericRepository(ToDoListDbContext dbContext)
        {
            this._context = dbContext;
        }
        public void Add(T entity)
        {
            _context.Set<T>().AddAsync(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
             _context.Set<T>().AddRangeAsync(entities);
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
        public void Remove(T entity)
        {
             _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
    }
}
