using System;
using System.Collections.Generic;
using System.Text;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Domain.Interfaces
{
    public interface IToDoRepository : IGenericRepository<ToDo> { }
}
