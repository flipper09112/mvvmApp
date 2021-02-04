using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Products
{
    public interface IAddProductToOrderService
    {
        //For change daily orders
        List<DayOfWeek> ListDaysToPast { get; set; }

        //For add products
        List<Product> ProductsSelected { get; }
        DayOfWeek AddProductDay { get; set; }
        void AddProduct(Product product);
        void Clear();
    }
}
