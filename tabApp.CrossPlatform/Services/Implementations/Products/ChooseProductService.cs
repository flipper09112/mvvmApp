using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public class ChooseProductService : IChooseProductService
    {
        private Product _product;
        public Product Product => _product;

        public EditTypeEnum EditType { get; set; }

        public void SelectProduct(Product product)
        {
            _product = product;
        }
    }

    public enum EditTypeEnum
    {
        Buy,
        Sell
    }
}
