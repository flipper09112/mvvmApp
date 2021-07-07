using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public interface IChooseProductService
    {
        Product Product { get; }
        void SelectProduct(Product product);
    }
}
