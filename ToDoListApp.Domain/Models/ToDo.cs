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

        //public override string ToString()
        //{
        //    return $"Id: {Id,-5}\nTitle: {Title,-15}\n" +
        //        $"Description: {Description}\nCompleted: {Status}\n" +
        //        $"Due Date: {DueDate.Date:dd:MM:yyyy}";
        //}
    }
}