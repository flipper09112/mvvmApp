using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models.Notifications;

namespace tabApp.Core.Services.Interfaces.Notifications
{
    public interface INotificationsManagerService
    {
        List<Notification> AllNotifications { get; set; }
        List<Notification> TodayNotifications { get; }

        void SetNotifications(List<Notification> lists);
        bool HasNotificationSameDaySameClient(Notification notification);
    }
}
