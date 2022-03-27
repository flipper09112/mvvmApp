using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Products
{
    public class ProductsManagerService : IProductsManagerService
    {
        private IClientsManagerService _clientsManagerService;

        private List<Product> _productsList;

        public List<Product> ProductsList => _productsList;
        public PriceChangeDate LastPricesDateChange { get; private set; }

        public ProductsManagerService(IClientsManagerService clientsManagerService)
        {
            _clientsManagerService = clientsManagerService;
        }

        public List<Client> ClientsWithTables { 
            get
            {
                List<Client> clients = new List<Client>();

                foreach (var item in _productsList[0].ReSaleValues ?? new List<ReSaleValues>())
                {
                    var cli = _clientsManagerService.ClientsList.Find(client => client.Id == item.ClientId);
                    if(cli != null)
                        clients.Add(cli);
                }
                return clients;
            }
        }

        public double GetProductAmmount(int clientId, Product product)
        {
            foreach(var item in product.ReSaleValues ?? new List<ReSaleValues>())
            {
                if (clientId == item.ClientId)
                    return item.Value;
            }

            return product.PVP;
        }

        public Product GetProductByClosestName(string productName)
        {
            Product produto = null;
            double sim = 0;
            double simProv;

            if (productName.Equals("Bolos pastelaria") || productName.Length == 0)
                return null;

            foreach (Product item in ProductsList)
            {
                simProv = Compare(productName, item.Name);
                if (simProv > sim)
                {
                    sim = simProv;
                    produto = item;
                }
            }
            return produto;
        }

        private double Compare(string name, string compareName)
        {
            int length = name.Length;
            int comLength = compareName.Length;
            int max = length;
            int min = comLength;
            int result = 0;
            if (length < comLength)
            {
                max = comLength;
                min = length;
            }

            for (int index = 0; index < min; index++)
            {
                if (name[index] == compareName[index])
                {
                    result++;
                }
            }
            return (double)(result) / (double)(max);
        }

        public Product GetProductById(int productId)
        {
            return _productsList.Find(item => item.Id == productId);
        }

        public string GetProductNameById(int productId)
        {
            return _productsList.Find(item => item.Id == productId).Name;
        }

        public void SetProducts(List<Product> productsList)
        {
            _productsList = productsList;
        }

        public List<ProductTypeEnum> GetAllProductsTypes()
        {
            List<ProductTypeEnum> list = new List<ProductTypeEnum>();

            foreach (var prod in _productsList)
            {
                if (!list.Contains(prod.ProductType))
                    list.Add(prod.ProductType);
            }

            return list;
        }

        public int GetUniqueId()
        {
            int max = 0;
            foreach(var prod in _productsList)
            {
                if (prod.Id > max)
                    max = prod.Id;
            }
            return max + 1;
        }

        public string GetDailyOrderDesc(Client client)
        {
            DateTime todayDate = DateTime.Today;
            string details = "";
            bool first = true;

            if (!client.Active) return "Cliente Inativado";

            if (_clientsManagerService.HasOrderThisDate(client, todayDate) is ExtraOrder extraOrder)
            {
                List<DailyOrderDetails> list = new List<DailyOrderDetails>();

                foreach (var item in extraOrder.AllItems)
                {
                    list.Add(new DailyOrderDetails() { 
                        ProductId = item.ProductId,
                        Ammount = item.Ammount
                    });
                }

                foreach (var item in _clientsManagerService.GetTodayDailyOrder(client, todayDate.DayOfWeek).AllItems)
                {
                    DailyOrderDetails dailyOrderItem = list.Find(prod => prod.ProductId == item.ProductId);

                    if(dailyOrderItem != null)
                    {
                        dailyOrderItem.Ammount += item.Ammount;
                    }
                    else
                    {
                        list.Add(new DailyOrderDetails()
                        {
                            ProductId = item.ProductId,
                            Ammount = item.Ammount
                        });
                    }
                }

                foreach (var item in list)
                {
                    Product product = GetProductById(item.ProductId);
                    details += first ? "" : "\n" + product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2"));
                    first = false;
                }
            }
            else
            {
                foreach (var item in _clientsManagerService.GetTodayDailyOrder(client, todayDate.DayOfWeek).AllItems)
                {
                    Product product = GetProductById(item.ProductId);
                    details += (first ? "" : "\n") + product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2"));
                    first = false;
                }
            }

            if (details == "") return "Nenhum Produto";

            return details;
        }

        public void SetGetPriceChangeDate(List<PriceChangeDate> priceChangeDates)
        {
            if (priceChangeDates == null || priceChangeDates.Count == 0)
                LastPricesDateChange = null;
            else
                LastPricesDateChange = priceChangeDates[0];
        }

        public void UpdateLastPricesDateChange(DateTime date)
        {
            if(LastPricesDateChange == null)
            {
                LastPricesDateChange = new PriceChangeDate();
            }

            LastPricesDateChange.Date = date;
        }
    }
}
