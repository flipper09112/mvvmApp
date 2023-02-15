using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Notifications;

namespace tabApp.Core.Services.Implementations.Notifications
{
    public class NotificationsManagerService : INotificationsManagerService
    {
        private IClientsManagerService _clientsManagerService;

        public List<Notification> AllNotifications { get; set; }

        public List<Notification> TodayNotifications
        {
            get
            {
                return GetTodayNotifications();
            }
        }

        public NotificationsManagerService(IClientsManagerService clientsManagerService)
        {
            _clientsManagerService = clientsManagerService;
        }

        private List<Notification> GetTodayNotifications()
        {
            var todayItems = AllNotifications?.FindAll(item => item.AlertDay.Date == DateTime.Today);

            todayItems.RemoveAll(item => _clientsManagerService.GetClientById(item.ClientId) == null);

            return todayItems?.OrderBy(item => _clientsManagerService.GetClientById(item.ClientId).Position).ToList<Notification>();
        }

        public void SetNotifications(List<Notification> lists)
        {
            AllNotifications = lists;
        }

        public bool HasNotificationSameDaySameClient(Notification notification)
        {
            foreach(Notification not in AllNotifications)
            {
                if (not.AlertDay.Date == notification.AlertDay.Date
                    && not.ClientId == notification.ClientId
                    && not.NotificationType == notification.NotificationType)
                    return true;
            }

            return false;
        }
    }
}
