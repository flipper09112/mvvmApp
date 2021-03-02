using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Snooze
{
    public class SnoozeViewModel : BaseViewModel
    {
        private IOrdersManagerService _ordersManagerService;
        private IProductsManagerService _productsManagerService;

        public SnoozeViewModel(IOrdersManagerService ordersManagerService,
                               IProductsManagerService productsManagerService)
        {
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
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
    }
}
