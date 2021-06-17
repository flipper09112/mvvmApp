using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Products;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class EditProductViewModel : BaseViewModel
    {
        private IChooseProductService _chooseProductService;
        public Product ProductSelected => _chooseProductService.Product;
        public EditProductViewModel(IChooseProductService chooseProductService)
        {
            _chooseProductService = chooseProductService;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
