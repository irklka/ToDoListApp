using System;

namespace ToDoListApp.Client.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}