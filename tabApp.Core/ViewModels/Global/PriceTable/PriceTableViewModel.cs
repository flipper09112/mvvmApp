using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels.Global.PriceTable;

namespace tabApp.Core.ViewModels.Global
{
    public class PriceTableViewModel : BaseViewModel
    {
        private IProductsManagerService _productsManagerService;
        private IMvxNavigationService _navigationService;
        private IPriceTableFilterService _priceTableFilterService;

        public MvxCommand ShowPriceTableFilterCommand;

        public bool HasFilter => _priceTableFilterService.HasFilter;
        public Client ClientFilter => _priceTableFilterService.ClientSelected;

        public PriceTableViewModel(IProductsManagerService productsManagerService,
                                   IMvxNavigationService navigationService,
                                   IPriceTableFilterService priceTableFilterService)
        {
            _productsManagerService = productsManagerService;
            _navigationService = navigationService;
            _priceTableFilterService = priceTableFilterService;

            ShowPriceTableFilterCommand = new MvxCommand(ShowPriceTableFilter);
        }

        private async void ShowPriceTableFilter()
        {
            await _navigationService.Navigate<PriceTableFilterViewModel>();
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
