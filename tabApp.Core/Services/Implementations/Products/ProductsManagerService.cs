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
    }
}
