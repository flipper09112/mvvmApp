using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Products
{
    public interface IProductsManagerService
    {
        List<Product> ProductsList { get; }
        List<Client> ClientsWithTables { get; }

        void SetProducts(List<Product> productsList);
        Product GetProductById(int productId);
        string GetProductNameById(int productId);
        List<ProductTypeEnum> GetAllProductsTypes();
        double GetProductAmmount(int clientId, Product product);
        Product GetProductByClosestName(string productName);
        int GetUniqueId();
        string GetDailyOrderDesc(Client client);
    }
}
