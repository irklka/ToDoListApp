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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToDoController/Create
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
           //var list = await _unitOfWork.ToDoLists.GetByIdAsync(todo.ToDoListId);
           //todo.ToDoList = list.ToDoListDomainToClientModel();
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
                return RedirectToAction("Index", "ToDoListController");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ToDoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction("Index", "ToDoListController");
            }
            catch
            {
                return View();
            }
        }
    }
}
