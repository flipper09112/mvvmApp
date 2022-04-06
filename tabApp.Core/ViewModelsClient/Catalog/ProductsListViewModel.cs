using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient.Catalog
{
    public class ProductsListViewModel : BaseViewModel
    {
        private IGetProductsRequest _getProductsRequest;
        private IChooseProductService _chooseProductService;
        private IMvxNavigationService _navigationService;
        private IDialogService _dialogService;

        public MvxCommand<ProductModel> ShowSelectedProductCommand { get; private set; }

        public ProductsListViewModel(IMvxNavigationService navigationService, 
                                     IGetProductsRequest getProductsRequest,
                                     IChooseProductService chooseProductService,
                                     IDialogService dialogService)
        {
            _getProductsRequest = getProductsRequest;
            _chooseProductService = chooseProductService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            ShowSelectedProductCommand = new MvxCommand<ProductModel>(ShowSelectedProduct);
        }

        private async void ShowSelectedProduct(ProductModel product)
        {
            _chooseProductService.ProductSelectedId = product.ProductId;
            await _navigationService.Navigate<ProductDetailsViewModel>();
        }

        private GetProductsOutput _productsList;
        public GetProductsOutput ProductsList
        {
            get => _productsList;
            set
            {
                _productsList = value;
                RaisePropertyChanged(nameof(ProductsList));
            } 
        }
        
        public override async void Appearing()
        {
            try
            {
                IsBusy = true;

                if (ProductsList != null && ProductsList.products != null && ProductsList.products.Count > 0)
                {
                    return;
                }

                var result = await _getProductsRequest.Send(new GetProductsInput() { Id = _chooseProductService.ProductTypeSelected.ToString() });

                if (result.Success)
                {
                    ProductsList = result;
                }
                else
                {
                    _dialogService.ShowConfirmDialog(result.ErrorMessage ?? "Ocorreu um erro. Tente novamente", "Ok", ConfirmAction);
                }
            }
            finally
            {
                IsBusy = false;
            }
           
        }

        private void ConfirmAction(bool obj)
        {
            _navigationService.Close(this); 
        }

        public override void DisAppearing()
        {
        }
    }
}
