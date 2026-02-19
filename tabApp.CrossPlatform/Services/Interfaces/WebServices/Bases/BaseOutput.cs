using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.WebServices.Bases
{
    public class BaseOutput
    {
        public string Error { get; set; }
        public bool Success => string.IsNullOrEmpty(Error);
    }
}
