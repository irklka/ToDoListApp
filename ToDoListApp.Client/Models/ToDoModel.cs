using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ToDoListApp.Domain.Enums;

namespace ToDoListApp.Client.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "ToDo's Title")]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Display(Name = "ToDo's Status")]
        public ToDoStatus Status { get; set; }

        [Display(Name = "Date Of Creation")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Display(Name = "Due Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }
        public int ToDoListId { get; set; }
        
        [JsonIgnore]
        public ToDoListModel ToDoList { get; set; }

        public string PrettyDateTime()
        {
            return DueDate.HasValue ? DueDate.Value.ToString("dd/MM/yy HH:mm") : "No Due Date Yet";
        }

        public string PrettyEnum()
        {
            return Status switch
            {
                ToDoStatus.Completed => "Completed",
                ToDoStatus.InProgress => "In Progress",
                ToDoStatus.NotStarted => "Not Started",
                _ => "No Enum Selected",
            };
        }
    }
}