using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Orders
{
    public class OrdersManagerService : IOrdersManagerService
    {

        private readonly IProductsManagerService _productsManagerService;
        private readonly IClientsManagerService _clientsManagerService;

        public OrdersManagerService(IProductsManagerService productsManagerService, IClientsManagerService clientsManagerService)
        {
            _productsManagerService = productsManagerService;
            _clientsManagerService = clientsManagerService;
        }

        public List<(Client Client, ExtraOrder ExtraOrder)> TodayOrders
        {
            get
            {
                List<(Client Client, ExtraOrder ExtraOrder)> orders = new List<(Client Client, ExtraOrder ExtraOrder)>();

                foreach(var client in _clientsManagerService.ClientsList)
                {
                    foreach(var extraorder in client.ExtraOrdersList)
                    {
                        //TODO add constraicts
                        orders.Add((client, extraorder));
                    }
                }

                return orders;
            }
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
