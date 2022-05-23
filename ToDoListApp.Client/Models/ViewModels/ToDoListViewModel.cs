using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Client.Models.ViewModels
{
    public class ToDoListViewModel
    {
        public ToDoListModel ToDoList { get; set; }

        [Display(Name = "Hide Completed ToDo's")]
        public bool HideCompleted { get; set; }                   
    }
}
