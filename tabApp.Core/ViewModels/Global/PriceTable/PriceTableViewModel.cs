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
        private IChooseProductService _chooseProductService;

        public EventHandler ShowLongPressOptions;

        public MvxCommand ShowPriceTableFilterCommand;
        public MvxCommand ShowPriceTableConfigurationCommand;
        public MvxCommand<Product> LongPressCommand; 
        public MvxCommand EditProductCommand;
        public MvxCommand EditProductBuyCommand;

        public bool HasFilter => _priceTableFilterService.HasFilter;
        public Client ClientFilter => _priceTableFilterService.ClientSelected;

        public List<LongPressItem> LongPressItemsList { get; set; }
        public PriceTableViewModel(IProductsManagerService productsManagerService,
                                   IMvxNavigationService navigationService,
                                   IPriceTableFilterService priceTableFilterService,
                                   IChooseProductService chooseProductService)
        {
            _productsManagerService = productsManagerService;
            _navigationService = navigationService;
            _priceTableFilterService = priceTableFilterService;
            _chooseProductService = chooseProductService;

            ShowPriceTableFilterCommand = new MvxCommand(ShowPriceTableFilter);
            ShowPriceTableConfigurationCommand = new MvxCommand(ShowPriceTableConfiguration);
            LongPressCommand = new MvxCommand<Product>(LongPress);
            EditProductCommand = new MvxCommand(EditProduct);
            EditProductBuyCommand = new MvxCommand(EditProductBuy);
        }

        private async void EditProductBuy()
        {
            _chooseProductService.EditType = EditTypeEnum.Buy;
            await _navigationService.Navigate<EditProductCostValuesViewModel>();
        }

        private async void EditProduct()
        {
            _chooseProductService.EditType = EditTypeEnum.Sell;
            await _navigationService.Navigate<EditProductViewModel>();
        }

        private void LongPress(Product product)
        {
            _chooseProductService.SelectProduct(product);
            ShowLongPressOptions?.Invoke(null, null);
        }

        private async void ShowPriceTableConfiguration()
        {
            await _navigationService.Navigate<PriceTableConfigurationViewModel>();
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

        public Product ProductSelected { get; private set; }

        public bool ContainsInsensitive(string source, string search)
        {
            return (new CultureInfo("pt-PT").CompareInfo).IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        public override void Appearing()
        {
            LongPressItemsList = GetLongPressItems();
        }

        private List<LongPressItem> GetLongPressItems()
        {
            var items = new List<LongPressItem>();

            items.Add(new LongPressItem() { 
                Name = "Editar valores (Venda)",
                Command = EditProductCommand
            });

            items.Add(new LongPressItem()
            {
                Name = "Editar valores (Compra)",
                Command = EditProductBuyCommand
            });

            return items;
        }

        public override void DisAppearing()
        {
        }
    }
}
