using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferEngine.Models.Error
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }

        public ErrorViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}