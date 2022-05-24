using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;
using System.Text.Json;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Client.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ILogger<ToDoListController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ToDoListController(ILogger<ToDoListController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: ToDoListController
        public async Task<ViewResult> Index()
        {
            var toDoLists = await _unitOfWork.ToDoLists.GetAllAsync();
            _unitOfWork.Complete();

            return View(toDoLists.Where(x => x.IsVisible).ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController
        public async Task<ViewResult> InvisibleLists()
        {
            var toDoLists = await _unitOfWork.ToDoLists.GetAllAsync();
            _unitOfWork.Complete();

            return View("Index", toDoLists.Where(x => !x.IsVisible).ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController/Details/5
        public async Task<IActionResult> Details(int id, [FromQuery] bool hideCompleted, bool dueToday)
        {
            if (id <= 0)
            {
                return View();
            }
            var toDoList = await _unitOfWork.ToDoLists.GetByIdAsync(id);
            if (toDoList == null)
            {
                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            return View(new ToDoListViewModel()
            {
                ToDoList = toDoList.ToDoListDomainToClientModel(),
                HideCompleted = hideCompleted,
                DueToday = dueToday
            });
        }

        // GET: ToDoListController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoListModel list)
        {
            if (list == null || !ModelState.IsValid) return RedirectToAction(nameof(Error));

            try
            {
                await _unitOfWork.ToDoLists.Add(list.ToDoListClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

        }

        // GET: ToDoListController/Edit/5
        public async Task<ViewResult> Edit(int id)
        {
            if (id < 0)
            {
                return View();
            }
            var list = await _unitOfWork.ToDoLists.GetByIdAsync(id);

            if (list == null)
            {
                return View();
            }
            return View(list.ToDoListDomainToClientModel());
        }

        // POST: ToDoListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ToDoListModel list)
        {
            try
            {
                _unitOfWork.ToDoLists.Update(list.ToDoListClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Error), 404);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var list = await _unitOfWork.ToDoLists.GetByIdAsync(id);
                var todos = await _unitOfWork.ToDo.GetTodosForToDoListWithId(id);
                _unitOfWork.ToDo.RemoveRange(todos);
                await _unitOfWork.ToDoLists.Remove(list);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Error), 404);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Copy(int id)
        {
            var list = await _unitOfWork.ToDoLists.GetToDoListWithToDosAsync(id);
            var jsonTodoList = JsonSerializer.Serialize(list.ToDoListDomainToClientModel());
            var jsonTodos = from todo in list.ToDos.ListOfToDosDomainToClientModel()
                            select JsonSerializer.Serialize(todo);
            return View(nameof(Copy), new ToDoViewModel{ JsonToDoList = jsonTodoList, JsonTodos = jsonTodos });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorViewModel { StatusCode = statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
