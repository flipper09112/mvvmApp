using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public interface IChooseProductService
    {
        int ProductSelectedId { get; set; }
        Product Product { get; }
        EditTypeEnum EditType { get; set; }
        ProductTypeEnum ProductTypeSelected { get; set; }

        void SelectProduct(Product product);
    }
}
