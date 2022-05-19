using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Domain.Interfaces
{
    public interface IToDoListRepository : IGenericRepository<ToDoList>
    {
        Task<ToDoList> GetToDoListWithToDosAsync(int id);

        Task<IEnumerable<ToDoList>> GetRecentlyAddedToDoListsAsync(int count);
    }
}
