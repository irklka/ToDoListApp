using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Client.Mappers;

namespace ToDoListApp.Client.Components
{
    public class TodosViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public TodosViewComponent(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id, bool hideCompleted = false, bool dueToday = false)
        {
            var todos = await _unitOfWork.ToDo.GetTodosForToDoListWithId(id);
            if (todos.Count == 0)
            {
                return View(todos.ListOfToDosDomainToClientModel());
            }
            else if (hideCompleted)
            {
                return View(todos.Where(x => x.Status != 0).ListOfToDosDomainToClientModel());
            }
            else if (dueToday)
            {
                return View(todos.Where(x => x.DueDate.HasValue).Where(x => x.DueDate.Value.Date == System.DateTime.Today).ListOfToDosDomainToClientModel());
            }
            else
            {
                return View(todos.ListOfToDosDomainToClientModel());
            }
        }
    }
}
