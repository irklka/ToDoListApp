using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoListApp.Client.Mappers;
using ToDoListApp.Client.Models;
using ToDoListApp.Client.Models.ViewModels;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddNewToDoList(ToDoListModel toDoList)
        {
            _unitOfWork.ToDoLists.Add(
                new ToDoList
                {
                    Title = toDoList.Title,
                    IsVisible = toDoList.IsVisible,
                    CreationDate = DateTime.Now
                });
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Lists()
        {
            var lists = await _unitOfWork.ToDoLists.GetAllAsync();
            return View(lists.ListOfToDoListsDomainToClientModel());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
