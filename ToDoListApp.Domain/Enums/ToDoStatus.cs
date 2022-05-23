using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDoListApp.Domain.Enums
{
    public enum ToDoStatus
    {
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Not Started")]
        NotStarted
    }
}
