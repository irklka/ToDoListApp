
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToDoListApp.Client.Models;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Client.Mappers
{
    public static class DTOToViewModel
    {
        public static ToDoModel ToDoDomainToClientModel(this ToDo toDo)
        {
            return new ToDoModel()
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                CreationDate = toDo.CreationDate,
                DueDate = toDo.DueDate,
                Status = toDo.Status
            };
        }

        public static ToDoListModel ToDoListDomainToClienModel(this ToDoList toDoList)
        {
            return new ToDoListModel()
            {
                Id = toDoList.Id,
                Title = toDoList.Title,
                IsVisible = toDoList.IsVisible,
                CreationDate = toDoList.CreationDate,
                ToDos = toDoList.ToDos != null ? 
                (from td in toDoList.ToDos select td.ToDoDomainToClientModel()).ToArray() : null,
            };
        }

        public static IEnumerable<ToDoListModel> ListOfToDoListsDomainToClientModel(this IEnumerable<ToDoList> enumer){
            return enumer.Select(x => x.ToDoListDomainToClienModel());
        }
        public static IEnumerable<ToDoModel> ListOfToDosDomainToClientModel(this IEnumerable<ToDo> enumer)
        {
            return enumer.Select(x => x.ToDoDomainToClientModel());
        }
    }
}
