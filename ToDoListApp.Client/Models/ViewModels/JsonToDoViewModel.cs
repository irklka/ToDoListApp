using System.Collections.Generic;

namespace ToDoListApp.Client.Models.ViewModels
{
    public class JsonToDoViewModel
    { 
        public string JsonToDoList { get; set; }
        public IEnumerable<string> JsonTodos { get; set; }
    }
}
