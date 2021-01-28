using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Products
{
    public class AddProductToOrderService : IAddProductToOrderService
    {
        private List<Product> _productsSelected = new List<Product>();
        public List<Product> ProductsSelected => _productsSelected;

        public void AddProduct(Product product)
        {
            _productsSelected.Add(product);
        }

        public void Clear()
        {
            _productsSelected.Clear();
        }
    }
}
