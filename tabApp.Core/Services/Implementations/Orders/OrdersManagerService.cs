using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Orders
{
    public class OrdersManagerService : IOrdersManagerService
    {

        private readonly IProductsManagerService _productsManagerService;

        public OrdersManagerService(IProductsManagerService productsManagerService)
        {
            _productsManagerService = productsManagerService;
        }

        public double GetValue(int clientId, DailyOrder dailyOrder)
        {
            double ammount = 0;

            foreach(var item in dailyOrder.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);

                if (product == null)
                    return -1;

                ammount += item.Ammount * _productsManagerService.GetProductAmmount(clientId, product);
            }

            return ammount;
        }

        public double WeekAmmount(Client client)
        {
            double weekValue = 0;
            foreach(var item in client.DailyOrders)
            {
                weekValue += GetValue(client.Id, item);
            }
            return weekValue;
        }
    }
}
