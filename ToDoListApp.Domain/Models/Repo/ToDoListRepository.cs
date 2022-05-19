using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Domain.Models.Repo
{
    public class ToDoListRepository : GenericRepository<ToDoList>, IToDoListRepository
    {
        public ToDoListRepository(ToDoListDbContext context) : base(context) {}

        public async Task<ToDoList> GetToDoListWithToDosAsync(int id)
        {
            return await _context.ToDoLists.Include(x => x.ToDos).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ToDoList>> GetRecentlyAddedToDoListsAsync(int count)
        {
            return await _context.ToDoLists.OrderByDescending(d => d.CreationDate).Take(count).ToListAsync();
        }
    }
}
