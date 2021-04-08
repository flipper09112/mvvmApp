using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.Notifications
{
    public class NotificationType
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public MvxCommand Action { get; set; }
    }
}
