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

            _logger.LogInformation($"All items retrieved. Controller{nameof(Index)}");

            return View(toDoLists.Where(x => x.IsVisible).ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController
        public async Task<ViewResult> InvisibleLists()
        {
            var toDoLists = await _unitOfWork.ToDoLists.GetAllAsync();
            _unitOfWork.Complete();

            _logger.LogInformation($"Invisible items retrieved. Controller{nameof(InvisibleLists)}");

            return View("Index", toDoLists.Where(x => !x.IsVisible).ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController/Details/5
        public async Task<ActionResult> Details(int id, [FromQuery] bool hideCompleted, bool dueToday)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Additional parameters hideCompleted:{hideCompleted} dueToday:{dueToday}. Controller{nameof(Details)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            var toDoList = await _unitOfWork.ToDoLists.GetByIdAsync(id);
            
            if (toDoList == null)
            {
                _logger.LogError($"Item with {id} was not found. Additional parameters hideCompleted:{hideCompleted} dueToday:{dueToday}. Controller{nameof(Details)}");
                
                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            _logger.LogInformation($"Item with {id} was found. Additional parameters hideCompleted:{hideCompleted} dueToday:{dueToday}. Controller{nameof(Details)}");

            return View(new ToDoListViewModel()
            {
                ToDoList = toDoList.ToDoListDomainToClientModel(),
                HideCompleted = hideCompleted,
                DueToday = dueToday
            });
        }

        // GET: ToDoListController/Create
        public ViewResult Create()
        {
            _logger.LogInformation($"View called. Controller{nameof(Create)}");

            return View();
        }

        // POST: ToDoListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Create(ToDoListModel list)
        {
            if (list == null || !ModelState.IsValid)
            {
                _logger.LogError($"List is null or Model State is invalid IsValid:{ModelState.IsValid}. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error));
            }

            try
            {
                await _unitOfWork.ToDoLists.Add(list.ToDoListClientToDomainModel());
                _unitOfWork.Complete();

                _logger.LogInformation($"Item was added. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error during items addition. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

        }

        // GET: ToDoListController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            var list = await _unitOfWork.ToDoLists.GetByIdAsync(id);

            if (list == null)
            {
                _logger.LogError($"item with id:{id} was not found. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            _logger.LogInformation($"Item with id:{id} was found. Controller{nameof(Edit)}");

            return View(list.ToDoListDomainToClientModel());
        }

        // POST: ToDoListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Edit(int id, ToDoListModel list)
        {
            if (id <= 0 || list == null || !ModelState.IsValid)
            {
                _logger.LogError($"Id:{id}, item or ModelState was invalid. Isvalid{ModelState.IsValid}. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            try
            {
                _unitOfWork.ToDoLists.Update(list.ToDoListClientToDomainModel());
                _unitOfWork.Complete();

                _logger.LogInformation($"Item with id:{id} was updated. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error during updating item with id:{id}. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), 404);
            }
        }

        // POST: ToDoListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            try
            {
                var list = await _unitOfWork.ToDoLists.GetByIdAsync(id);
                var todos = await _unitOfWork.ToDo.GetTodosForToDoListWithId(id);
                
                if(list == null || todos == null)
                {
                    _logger.LogError($"Error during deleting item with id:{id}. Controller{nameof(Delete)}");

                    return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
                }

                _unitOfWork.ToDo.RemoveRange(todos);
                await _unitOfWork.ToDoLists.Remove(list);
                _unitOfWork.Complete();

                _logger.LogInformation($"Item with id:{id} was deleted with its subitems. Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error during deleting item with id:{id}. Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), 404);
            }
        }

        [HttpGet]
        public async Task<ViewResult> Copy(int id)
        {
            if(id <= 0)
            {
                _logger.LogError($"Error during copying item with id:{id}. Controller{nameof(Copy)}");
                
                return View();
            }
            var list = await _unitOfWork.ToDoLists.GetToDoListWithToDosAsync(id);

            if(list == null)
            {
                _logger.LogError($"Error during copying item with id:{id}. Controller{nameof(Copy)}");

                return View();
            }

            var jsonTodoList = JsonSerializer.Serialize(list.ToDoListDomainToClientModel());
            var jsonTodos = from todo in list.ToDos.ListOfToDosDomainToClientModel()
                            select JsonSerializer.Serialize(todo);

            _logger.LogInformation($"Item with id:{id} was serialized with its subitems. Controller{nameof(Copy)}");

            return View(nameof(Copy), new JsonToDoViewModel { JsonToDoList = jsonTodoList, JsonTodos = jsonTodos });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            _logger.LogInformation($"Error called Status Code:{statusCode}. Controller{nameof(Error)}");

            return View(new ErrorViewModel { StatusCode = statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
