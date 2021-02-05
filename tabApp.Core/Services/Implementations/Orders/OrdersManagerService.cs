using System;
using System.Collections.Generic;
using System.Text;
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
                                if (p.ProductType == ProductTypeEnum.PastelariaIndividual)
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
                        if (_productsManagerService.GetProductById(item.ProductId).ProductType == ProductTypeEnum.PastelariaIndividual)
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
                if (productModel.ProductType == ProductTypeEnum.PastelariaIndividual)
                    continue;

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
        public double GetValue(int clientId, List<(int ProductId, double Ammount)> allitems)
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
    }
}
