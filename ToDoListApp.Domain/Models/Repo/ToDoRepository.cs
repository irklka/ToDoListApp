using System;
using System.Collections.Generic;
using System.Text;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Domain.Models.Repo
{
    public class ToDoRepository : GenericRepository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ToDoListDbContext context) : base(context) { }
    }
}
