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

            var res = todos.ListOfToDosDomainToClientModel();
            if (!res.Any())
            {
                return View(res);
            }
            if (hideCompleted)
            {
                res = res.Where(x => x.Status != 0);
            }
            if (dueToday)
            {
                res = res.Where(x => x.DueDate.HasValue &&
                                            x.DueDate.Value.Date == System.DateTime.Today);
            }

            return View(res);
        }
    }
}
