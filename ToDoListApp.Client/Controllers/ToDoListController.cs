using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;

namespace ToDoListApp.Client.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ILogger<ToDoListController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ToDoListController(ILogger<ToDoListController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: ToDoListController
        public async Task<ViewResult> Index()
        {
            var toDoLists = await _unitOfWork.ToDoLists.GetAllAsync();
            _unitOfWork.Complete();

            return View(toDoLists.ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController/Details/5
        public async Task<ViewResult> Details(int id)
        {
            if(id <= 0)
            {
                return View();
            }
            var toDoList = await _unitOfWork.ToDoLists.GetByIdAsync(id);
            _unitOfWork.Complete();

            return View(toDoList.ToDoListDomainToClientModel());
        }

        // GET: ToDoListController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoListModel list)
        {
            if (list == null || !ModelState.IsValid) return StatusCode(400);

            try
            {
                await _unitOfWork.ToDoLists.Add(list.ToDoListClientToDomainModel());
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                return StatusCode(404);
            }
        }
    }
}
