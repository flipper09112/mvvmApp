using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels
{
    public class ChooseProductViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IAddProductToOrderService _addProductToOrderService;

        public MvxCommand<Product> SelectProductCommand;
        public ChooseProductViewModel(IMvxNavigationService navigationService, IProductsManagerService productsManagerService,
             IAddProductToOrderService addProductToOrderService)
        {
            _navigationService = navigationService;
            _productsManagerService = productsManagerService;
            _addProductToOrderService = addProductToOrderService;

            SelectProductCommand = new MvxCommand<Product>(SelectProduct);
        }

        private List<Product> _allProducts;
        public List<Product> AllProducts {
            get
            {
                if(SearchWord.Length == 0)
                    return _productsManagerService.ProductsList;

                return _productsManagerService.ProductsList.FindAll(item => ContainsInsensitive(item.Name, SearchWord));
            }
        }
        private string _searchWord = "";
        public string SearchWord { 
            get
            {
                return _searchWord;
            }
            set
            {
                _searchWord = value;
                RaisePropertyChanged(nameof(SearchWord));
            }
        }

        private void SelectProduct(Product product)
        {
            _addProductToOrderService.AddProduct(product);
            _navigationService.Close(this);
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
