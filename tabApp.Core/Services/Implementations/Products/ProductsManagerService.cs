using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Products
{
    public class ProductsManagerService : IProductsManagerService
    {
        private List<Product> _productsList;

        public List<Product> ProductsList => _productsList;

        public double GetProductAmmount(int clientId, Product product)
        {
            foreach(var item in product.ReSaleValues ?? new List<(int Id, double Value)>())
            {
                if (clientId == item.Id)
                    return item.Value;
            }

            return product.PVP;
        }

        public Product GetProductById(int productId)
        {
            return _productsList.Find(item => item.Id == productId);
        }

        public void SetProducts(List<Product> productsList)
        {
            _productsList = productsList;
        }
    }
}
