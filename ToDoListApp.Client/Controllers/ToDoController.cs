using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Domain.Models.UnitOfWork;

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
            if (todo == null || !ModelState.IsValid) return StatusCode(400);

            try
            {
                await _unitOfWork.ToDo.Add(todo.ToDoClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction("Index", "ToDoList");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToDoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ToDoModel todo)
        {
            try
            {
                return RedirectToAction("Index", "ToDoList");
            }
            catch
            {
                return View();
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
                return StatusCode(404);
            }
        }
    }
}
