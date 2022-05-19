using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoListApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IToDoListRepository ToDoLists { get; }
        public IToDoRepository ToDo { get; }
        public int Complete();
    }
}