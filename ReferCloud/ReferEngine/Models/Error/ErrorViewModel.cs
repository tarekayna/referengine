using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferEngineWeb.Models.Error
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