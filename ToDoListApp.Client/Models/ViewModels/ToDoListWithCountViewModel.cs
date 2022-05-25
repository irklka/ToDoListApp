using System.Collections.Generic;

namespace ToDoListApp.Client.Models.ViewModels
{
    public class ToDoListWithCountViewModel
    {
        public IEnumerable<ToDoListModel> ToDoLists { get; set; }
        public int Count { get; set; }
    }
}
