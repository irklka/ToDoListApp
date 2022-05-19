using System;
using System.Collections.Generic;
using System.Text;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Domain.Models.Repo;

namespace ToDoListApp.Domain.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ToDoListDbContext _context;
        public UnitOfWork(ToDoListDbContext context)
        {
            _context = context;
            ToDoLists = new ToDoListRepository(_context);
            ToDo = new ToDoRepository(_context);
        }
        public IToDoListRepository ToDoLists { get; private set; }
        public IToDoRepository ToDo { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
