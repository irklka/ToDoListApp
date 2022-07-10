using System;
using System.Collections.Generic;

namespace ToDoListApp.Domain.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<ToDo> ToDos { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
