using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public interface IChooseProductService
    {
        Product Product { get; }
        EditTypeEnum EditType { get; set; }
        void SelectProduct(Product product);
    }
}
