﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;
using System.Diagnostics;

namespace ToDoListApp.Client.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ILogger<ToDoListController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private bool HideCompeted { get; set; }

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

            return View(toDoLists.ListOfToDoListsDomainToClientModel());
        }

        // GET: ToDoListController/Details/5
        public async Task<ViewResult> Details(int id)
        {
            if (id <= 0)
            {
                return View();
            }
            var toDoList = await _unitOfWork.ToDoLists.GetByIdAsync(id);
            _unitOfWork.Complete();

            return View( new ToDoListViewModel() { ToDoList = toDoList.ToDoListDomainToClientModel(), HideCompleted = this.HideCompeted });
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
                return RedirectToAction(nameof(Error), 404);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorViewModel { StatusCode = statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
