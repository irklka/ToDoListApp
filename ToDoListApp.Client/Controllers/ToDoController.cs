using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Client.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ToDoController(ILogger<ToDoController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: ToDoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if(id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Controller{nameof(Details)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            var todo = await _unitOfWork.ToDo.GetByIdAsync(id);

            if(todo == null)
            {
                _logger.LogError($"Item with {id} was not found. Controller{nameof(Details)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            _logger.LogInformation($"Item with {id} was found. Controller{nameof(Details)}");

            return View(todo.ToDoDomainToClientModel());
        }

        // GET: ToDoController/Create/5
        public ActionResult Create(int listId)
        {
            if (listId <= 0)
            {
                _logger.LogError($"Id:{listId} was invalid (was <= 0). Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            ViewBag.ListId = listId;

            _logger.LogInformation($"Controller called view for listId:{listId}. Controller{nameof(Create)}");

            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Create(ToDoModel todo)
        {
            if (todo == null || !ModelState.IsValid)
            {
                _logger.LogError($"item was invalid IsValid:{ModelState.IsValid}. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status403Forbidden });
            }

            try
            {
                await _unitOfWork.ToDo.Add(todo.ToDoClientToDomainModel());
                _unitOfWork.Complete(); 
                
                _logger.LogInformation($"item was added id:{todo.Id}. Controller{nameof(Create)}");

                return RedirectToAction("Details", "ToDoList", new { id = todo.ToDoListId });
            }
            catch
            {
                _logger.LogError($"Error occured during addition of item. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }
        }

        // GET: ToDoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            var todo = await _unitOfWork.ToDo.GetByIdAsync(id);

            if (todo == null)
            {
                _logger.LogError($"item with id:{id} was not found. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            _logger.LogInformation($"Item with id:{id} was found. Controller{nameof(Edit)}");

            return View(todo.ToDoDomainToClientModel());
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Edit(int id, ToDoModel todo)
        {
            if (id <= 0 || todo == null || !ModelState.IsValid)
            {
                _logger.LogError($"Id:{id}, item or ModelState was invalid. Isvalid{ModelState.IsValid}. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            try
            {
                _unitOfWork.ToDo.Update(todo.ToDoClientToDomainModel());
                _unitOfWork.Complete();

                _logger.LogInformation($"Item with id:{id} was updated. Controller{nameof(Edit)}");

                return RedirectToAction("Details", "ToDoList", new { id = todo.ToDoListId });
            }
            catch
            {
                _logger.LogError($"Error during updating item with id:{id}. Controller{nameof(Edit)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }
        }

        // Get: ToDoController/Delete/5
        public async Task<RedirectToActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was <= 0). Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            try
            {
                var todo = await _unitOfWork.ToDo.GetByIdAsync(id);

                if (todo == null)
                {
                    _logger.LogError($"Error during deleting item with id:{id}. Controller{nameof(Delete)}");

                    return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
                }

                await _unitOfWork.ToDo.Remove(todo);
                _unitOfWork.Complete();

                _logger.LogInformation($"Item with id:{id} was deleted. Controller{nameof(Delete)}");

                return RedirectToAction("Details", "ToDoList", new { id = todo.ToDoListId });
            }
            catch
            {
                _logger.LogError($"Error during deleting item with id:{id}. Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }
        }

        // POST: ToDoController/ShowDueToday
        public async Task<ViewResult> ShowDueToday()
        {
            var todo = await _unitOfWork.ToDo.FindAsync(x => x.DueDate.HasValue &&
                                            x.DueDate.Value.Date == System.DateTime.Today);
           
            _logger.LogInformation($"Item Due Today were retrieved. Controller{nameof(ShowDueToday)}");

            return View(todo.ListOfToDosDomainToClientModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorViewModel { StatusCode = statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
