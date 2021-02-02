using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
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

                foreach(var client in _clientsManagerService?.ClientsList ?? new List<Client>())
                {
                    foreach(var extraorder in client.ExtraOrdersList)
                    {
                        if(extraorder.OrderDay.Date == DateTime.Today)
                            orders.Add((client, extraorder));
                    }
                }
                return orders;
            }
        }
        public List<(Client Client, ExtraOrder ExtraOrder)> TomorrowOrders
        {
            get
            {
                List<(Client Client, ExtraOrder ExtraOrder)> orders = new List<(Client Client, ExtraOrder ExtraOrder)>();

                foreach (var client in _clientsManagerService?.ClientsList ?? new List<Client>())
                {
                    foreach (var extraorder in client.ExtraOrdersList)
                    {
                        if (extraorder.OrderDay.Date == DateTime.Today.AddDays(1))
                            orders.Add((client, extraorder));
                    }
                }
                return orders;
            }
        }

        public List<ProductAmmount> GetTotalOrder(DateTime dateTime)
        {
            List<ProductAmmount> items = new List<ProductAmmount>();
            foreach(var client in _clientsManagerService.ClientsList)
            {
                ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, dateTime);
                if(order == null)
                {
                    AddProductAmmountToList(items, ClientHelper.GetDailyOrder(dateTime.DayOfWeek, client).AllItems);
                } else
                {
                    if(order.IsTotal)
                    {
                        AddProductAmmountToList(items, order.AllItems);
                    } else
                    {
                        AddProductAmmountToList(items, order.AllItems);
                        AddProductAmmountToList(items, ClientHelper.GetDailyOrder(dateTime.DayOfWeek, client).AllItems);
                    }
                }
            }
            return items;
        }

        private void AddProductAmmountToList(List<ProductAmmount> items, List<(int ProductId, double Ammount)> dailyOrder)
        {
            foreach(var dailyItem in dailyOrder)
            {
                var listItem = items.Find(item => item.Product.Id == dailyItem.ProductId);
                Product productModel = _productsManagerService.GetProductById(dailyItem.ProductId);
                if (!productModel.Unity)
                {
                    items.Add(new ProductAmmount()
                    {
                        Product = productModel,
                        Ammount = dailyItem.Ammount
                    });
                }
                else if(listItem == null)
                {
                    items.Add(new ProductAmmount()
                    {
                        Product = productModel,
                        Ammount = dailyItem.Ammount
                    });
                }else
                {
                    listItem.Ammount += dailyItem.Ammount;
                }
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
