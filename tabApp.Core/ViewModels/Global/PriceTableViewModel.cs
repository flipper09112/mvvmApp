using System;
using System.Collections.Generic;
using System.Globalization;
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

        private List<Product> _allProducts;
        public List<Product> AllProducts
        {
            get
            {
                if (SearchProduct.Length == 0)
                    return _productsManagerService.ProductsList;


                return _productsManagerService.ProductsList.FindAll(item => ContainsInsensitive(item.Name, SearchProduct));
            }
        }

        private string _searchWord = "";
        public string SearchProduct
        {
            get
            {
                return _searchWord;
            }
            set
            {
                _searchWord = value;
                RaisePropertyChanged(nameof(SearchProduct));
            }
        }
        public bool ContainsInsensitive(string source, string search)
        {
            return (new CultureInfo("pt-PT").CompareInfo).IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
