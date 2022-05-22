using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Domain.Interfaces
{
    public interface IToDoRepository : IGenericRepository<ToDo> 
    {
        Task<List<ToDo>> GetTodosForToDoListWithId(int id);
    }
}
