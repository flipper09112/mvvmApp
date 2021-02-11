using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global
{
    public class PriceTableViewModel : BaseViewModel
    {
        private IProductsManagerService _productsManagerService;

        public PriceTableViewModel(IProductsManagerService productsManagerService)
        {
            _productsManagerService = productsManagerService;
        }

        public List<Product> AllProducts => _productsManagerService.ProductsList;

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
