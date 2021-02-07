using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class PrintPreview
    {
        public MvxCommand<PrintPreview> SetSelectedCommand { get; set; }
        public string Preview { get; }
        public bool Selected { get; set; }
        public string Name { get; }

        public PrintPreview(string preview, string name, MvxCommand<PrintPreview> setSelectedCommand = null, bool selected = false) 
        {
            SetSelectedCommand = setSelectedCommand; 
            Preview = preview;
            Selected = selected;
            Name = name;
        }
    }
}
