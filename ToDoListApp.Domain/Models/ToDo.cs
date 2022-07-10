using System;
using ToDoListApp.Domain.Enums;

namespace ToDoListApp.Domain.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ToDoStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
    }
}