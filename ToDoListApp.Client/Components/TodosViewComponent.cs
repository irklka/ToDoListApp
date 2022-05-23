using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Client.Mappers;
using System.Collections.Generic;
using ToDoListApp.Client.Models;

namespace ToDoListApp.Client.Components
{
    public class TodosViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public TodosViewComponent(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var todos = await _unitOfWork.ToDo.GetTodosForToDoListWithId(id);
            if (todos.Count == 0) return View(todos.ListOfToDosDomainToClientModel());
            return View(todos.ListOfToDosDomainToClientModel());
        }
    }
}
