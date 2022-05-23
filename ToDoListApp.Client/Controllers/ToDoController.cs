using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;
using ToDoListApp.Domain.Interfaces;

namespace ToDoListApp.Client.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoListController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ToDoController(ILogger<ToDoListController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        // GET: ToDoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var todo = await _unitOfWork.ToDo.GetByIdAsync(id);
            return View(todo.ToDoDomainToClientModel());
        }

        // GET: ToDoController/Create/5
        public ActionResult Create(int id)
        {
            ViewBag.ListId = id;
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoModel todo)
        {
            if (todo == null || !ModelState.IsValid) return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status403Forbidden });

            try
            {
                await _unitOfWork.ToDo.Add(todo.ToDoClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction("Index", "ToDoList");
            }
            catch
            {
                return RedirectToAction(nameof(Error), 404);
            }
        }

        // GET: ToDoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id < 0)
            {
                return View();
            }
            var todo = await _unitOfWork.ToDo.GetByIdAsync(id);

            if (todo == null)
            {
                return RedirectToAction(nameof(Error), 404);
            }
            return View(todo.ToDoDomainToClientModel());
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ToDoModel todo)
        {
            try
            {
                _unitOfWork.ToDo.Update(todo.ToDoClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction("Details", "ToDoList", new { id = todo.ToDoListId });
            }
            catch
            {
                return RedirectToAction(nameof(Error), 404);
            }
        }

        // POST: ToDoController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var todo = await _unitOfWork.ToDo.GetByIdAsync(id);
                await _unitOfWork.ToDo.Remove(todo);
                _unitOfWork.Complete();
                return RedirectToAction("Details", "ToDoList",new { id = todo.ToDoListId });
            }
            catch
            {
                return RedirectToAction(nameof(Error), 404);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorViewModel { StatusCode = statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
