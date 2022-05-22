using System.Collections.Generic;
using System.Linq;
using ToDoListApp.Client.Models;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Client.Mappers
{
    public static class ClientModelToDomainModelMappers
    {
        public static ToDo ToDoClientToDomainModel(this ToDoModel toDo)
        {
            return new ToDo()
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                CreationDate = toDo.CreationDate,
                DueDate = toDo.DueDate,
                Status = toDo.Status,
                ToDoListId = toDo.ToDoListId
            };
        }

        public static ToDoList ToDoListClientToDomainModel(this ToDoListModel toDoList)
        {
            return new ToDoList()
            {
                Id = toDoList.Id,
                Title = toDoList.Title,
                IsVisible = toDoList.IsVisible,
                CreationDate = toDoList.CreationDate,
                ToDos = toDoList.ToDos != null ?
                (from td in toDoList.ToDos select td.ToDoClientToDomainModel()).ToArray() : null,
            };
        }

        public static IEnumerable<ToDoList> ListOfToDoListsClientToDomainModel(this IEnumerable<ToDoListModel> enumer)
        {
            return enumer.Select(x => x.ToDoListClientToDomainModel());
        }
        public static IEnumerable<ToDo> ListOfToDosClientToDomainModel(this IEnumerable<ToDoModel> enumer)
        {
            return enumer.Select(x => x.ToDoClientToDomainModel());
        }
    }
}
