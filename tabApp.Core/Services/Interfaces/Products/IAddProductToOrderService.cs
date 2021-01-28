using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Products
{
    public interface IAddProductToOrderService
    {
        List<Product> ProductsSelected { get; }

        void AddProduct(Product product);

        void Clear();
    }
}
