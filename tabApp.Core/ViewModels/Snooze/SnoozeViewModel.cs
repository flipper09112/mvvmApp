using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Snooze
{
    public class SnoozeViewModel : BaseViewModel
    {
        private IOrdersManagerService _ordersManagerService;
        private IProductsManagerService _productsManagerService;
        private INotificationsManagerService _notificationManagerService;
        private IClientsManagerService _clientsManagerService;

        public List<Notification> NotificationsList => _notificationManagerService.TodayNotifications;

        public SnoozeViewModel(IOrdersManagerService ordersManagerService,
                               IProductsManagerService productsManagerService,
                               INotificationsManagerService notificationManagerService,
                               IClientsManagerService clientsManagerService)
        {
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
            _notificationManagerService = notificationManagerService;
            _clientsManagerService = clientsManagerService;
        }

        private List<(Client Client, ExtraOrder ExtraOrder)> _todayOrders;

        public List<(Client Client, ExtraOrder ExtraOrder)> TodayOrders
        {
            get
            {
                return _todayOrders;
            }
            set
            {
                _todayOrders = value;
            }
        }

        public Client GetClient(int clientId)
        {
            return _clientsManagerService.ClientsList.Find(item => item.Id == clientId);
        }

        public string GetOrderDesc(ExtraOrder obj)
        {
            string details = "";
            foreach (var item in obj.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);
                details += product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2")) + "\n";
            }
            return details;
        }

        public override void Appearing()
        {
            TodayOrders = _ordersManagerService.TodayOrders;
        }

        public override void DisAppearing()
        {
        }

        public Client GetClosestClient(double latitude, double longitude)
        {
            return _clientsManagerService.GetClosestClient(latitude, longitude);
        }

        public string GetDailyOrderDesc(Client client)
        {
            return _productsManagerService.GetDailyOrderDesc(client);
        }
    }
}
