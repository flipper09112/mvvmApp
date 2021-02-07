using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class ClientProfileField
    {
        public bool IsInt { get; internal set; }
        public bool IsDate { get; internal set; }
        public string Name { get; internal set; }
        public string IconName { get; internal set; }
        public string Value { get; internal set; }
        public string NewValue { get; set; }
        public MvxCommand RefreshSaveCommand { get; internal set; }
        public bool IsDouble { get; internal set; }
        public string Type { get; internal set; }
        public Product Product { get; internal set; }
    }
}
