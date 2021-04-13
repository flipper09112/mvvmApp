using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Notifications;

namespace tabApp.Core.Services.Implementations.Notifications
{
    public class NotificationsManagerService : INotificationsManagerService
    {
        public List<Notification> AllNotifications { get; set; }

        public List<Notification> TodayNotifications => AllNotifications?.FindAll(item => item.AlertDay.Date == DateTime.Today);

        public void SetNotifications(List<Notification> lists)
        {
            AllNotifications = lists;
        }
    }
}
