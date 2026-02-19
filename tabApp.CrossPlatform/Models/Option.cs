using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class Option
    {
        public MvxCommand Action { get; }
        public string Name { get; }
        public string ImageName { get; }

        public Option(MvxCommand action, string name, string imageName)
        {
            Action = action;
            Name = name;
            ImageName = imageName;
        }
    }
}
