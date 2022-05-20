using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Client.Models
{
    public class ToDoListModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "ToDo List Title")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required, Display(Name = "Visibility")]
        public bool IsVisible { get; set; } = true;

        [Display(Name = "Date Of Creation")]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public ICollection<ToDoModel> ToDos { get; set; }

        //public override string ToString()
        //{
        //    return $"Id: {Id,-5}\nTitle: {Title,-15}\nIsVisible: {IsVisible}";
        //}
        public string PrettyBool()
        {
            return IsVisible ? "Visible" : "Not Visible";
        }
    }
}
