using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Domain.Models.Repo
{
    public class ToDoRepository : GenericRepository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ToDoListDbContext context) : base(context) { }

        public ToDoListDbContext ToDoListDbContext => _context as ToDoListDbContext;

        public async Task<List<ToDo>> GetTodosForToDoListWithId(int id)
        {
            return await ToDoListDbContext.ToDos.Where(x => x.ToDoListId == id).ToListAsync();
        }
    }
}
