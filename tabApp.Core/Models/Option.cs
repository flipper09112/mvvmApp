using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class Option
    {
        public Action Action { get; }
        public string Name { get; }
        public string ImageName { get; }

        public Option(Action action, string name, string imageName)
        {
            Action = action;
            Name = name;
            ImageName = imageName;
        }
    }
}
