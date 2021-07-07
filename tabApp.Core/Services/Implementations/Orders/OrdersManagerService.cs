using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
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

                try
                {
                    foreach (var client in _clientsManagerService?.ClientsList ?? new List<Client>())
                    {
                        foreach (var extraorder in client.ExtraOrdersList)
                        {
                            if (extraorder.OrderDay.Date == DateTime.Today && !extraorder.StoreOrder)
                                orders.Add((client, extraorder));
                        }
                    }
                    return orders;

                } catch (InvalidOperationException e)
                {
                    Thread.Sleep(2000);
                    foreach (var client in _clientsManagerService?.ClientsList ?? new List<Client>())
                    {
                        foreach (var extraorder in client.ExtraOrdersList)
                        {
                            if (extraorder.OrderDay.Date == DateTime.Today && !extraorder.StoreOrder)
                                orders.Add((client, extraorder));
                        }
                    }
                    return orders;
                }
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
                        if (extraorder.OrderDay.Date == DateTime.Today.AddDays(1) && !extraorder.StoreOrder)
                            orders.Add((client, extraorder));
                    }
                }
                return orders;
            }
        }

        public List<CakeClientItem> CakesClientsTomorrow { 
            get
            {
                List<CakeClientItem> cakeClients = new List<CakeClientItem>();

                bool hasCake = false;
                List<(string ProductName, int Ammount)> products;
                foreach (var client in _clientsManagerService?.ClientsList ?? new List<Client>())
                {
                    hasCake = false;
                    products = new List<(string ProductName, int Ammount)>();

                    //Adicionar os produtos de uma encomenda extra
                    foreach (var extraorder in client.ExtraOrdersList)
                    {
                        if (extraorder.OrderDay.Date == DateTime.Today.AddDays(1))
                        {
                            foreach(var item in extraorder.AllItems)
                            {
                                Product p = _productsManagerService.GetProductById(item.ProductId);
                                if (p.ProductType == ProductTypeEnum.PastelariaIndividual || p.ProductType == ProductTypeEnum.SemiFrioIndividual)
                                {
                                    products.Add((p.Name, (int)item.Ammount));
                                }
                            }
                        }
                    }
                    //encomenda normal
                    foreach (var item in ClientHelper.GetDailyOrder(DateTime.Today.AddDays(1).DayOfWeek, client).AllItems)
                    {
                        Product p = _productsManagerService.GetProductById(item.ProductId);
                        if (_productsManagerService.GetProductById(item.ProductId).ProductType == ProductTypeEnum.PastelariaIndividual ||
                           _productsManagerService.GetProductById(item.ProductId).ProductType == ProductTypeEnum.SemiFrioIndividual)
                        {
                            products.Add((p.Name, (int)item.Ammount));
                        }
                    }

                    if(products.Count > 0) {
                        cakeClients.Add(new CakeClientItem(client, products));
                    }
                }

                return cakeClients;
            }
        }

        public List<ProductAmmount> GetTotalOrder(DateTime dateTime)
        {
            List<ProductAmmount> items = new List<ProductAmmount>();
            foreach(var client in _clientsManagerService.ClientsList)
            {
                if (!client.Active) continue;
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

        private void AddProductAmmountToList(List<ProductAmmount> items, List<DailyOrderDetails> allItems)
        {
            foreach (var dailyItem in allItems)
            {
                var listItem = items.Find(item => item.Product.Id == dailyItem.ProductId);
                Product productModel = _productsManagerService.GetProductById(dailyItem.ProductId);
                if (productModel.ProductType == ProductTypeEnum.PastelariaIndividual)
                    continue;

                if (/*!productModel.Unity*/ productModel.ProductType == ProductTypeEnum.None)
                {
                    items.Add(new ProductAmmount()
                    {
                        Product = productModel,
                        Ammount = dailyItem.Ammount
                    });
                }
                else if (listItem == null)
                {
                    items.Add(new ProductAmmount()
                    {
                        Product = productModel,
                        Ammount = dailyItem.Ammount
                    });
                }
                else
                {
                    listItem.Ammount += dailyItem.Ammount;
                }
            }
        }

        public double GetValue(int clientId, DailyOrder dailyOrder)
        {
            double ammount = 0;

            foreach(var item in dailyOrder?.AllItems ?? new List<DailyOrderDetails>())
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);

                if (product == null)
                    return -1;

                ammount += item.Ammount * _productsManagerService.GetProductAmmount(clientId, product);
            }

            return ammount;
        }
        public double GetValue(int clientId, List<DailyOrderDetails> allitems)
        {
            double ammount = 0;

            foreach (var item in allitems)
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

        public List<ProductAmmount> GetTotalOrderFromClient(Client fromClient, DateTime dateTime)
        {
            List<ProductAmmount> items = new List<ProductAmmount>();
            if (_clientsManagerService.ClientsList == null) return items;

            int position = _clientsManagerService.ClientsList.IndexOf(fromClient) == -1 ? 0 : _clientsManagerService.ClientsList.IndexOf(fromClient);

            foreach (var client in _clientsManagerService.ClientsList.Skip(position))
            {
                if (!client.Active) continue;
                ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, dateTime);
                if (order == null)
                {
                    AddProductAmmountToList(items, ClientHelper.GetDailyOrder(dateTime.DayOfWeek, client).AllItems);
                }
                else
                {
                    if (order.IsTotal)
                    {
                        AddProductAmmountToList(items, order.AllItems);
                    }
                    else
                    {
                        AddProductAmmountToList(items, order.AllItems);
                        AddProductAmmountToList(items, ClientHelper.GetDailyOrder(dateTime.DayOfWeek, client).AllItems);
                    }
                }
            }
            return items;
        }
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public List<ProductAmmount> GetTotalOrder(DateTime startDate, DateTime endDate)
        {
            List<ProductAmmount> items = new List<ProductAmmount>();

            foreach (var client in _clientsManagerService.ClientsList) {

                if (!client.Active) continue;

                ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, startDate);

                if(order != null)
                {
                    if (order.IsTotal)
                    {
                        AddProductAmmountToList(items, order.AllItems);
                        continue;
                    }
                    else
                    {
                        AddProductAmmountToList(items, order.AllItems);
                    }
                }

                foreach (DateTime day in EachDay(startDate, endDate))
                {
                    AddProductAmmountToList(items, ClientHelper.GetDailyOrder(day.DayOfWeek, client).AllItems);
                }
            }

            return items;
        }
    }
}
