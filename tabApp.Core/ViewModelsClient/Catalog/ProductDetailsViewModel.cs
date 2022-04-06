using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient.Catalog
{
    public class ProductDetailsViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IChooseProductService _chooseProductService;
        private IGetProductDetailsRequest _getProductDetailsRequest;
        private IDialogService _dialogService;

        public ProductDetailsViewModel(IMvxNavigationService navigationService,
                                       IChooseProductService chooseProductService,
                                       IGetProductDetailsRequest getProductDetailsRequest,
                                       IDialogService dialogService)
        {
            _navigationService = navigationService;
            _chooseProductService = chooseProductService;
            _getProductDetailsRequest = getProductDetailsRequest;
            _dialogService = dialogService;
        }

        private GetProductDetailsOutput _product;
        public GetProductDetailsOutput Product 
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged(nameof(Product));
            }
        }

        public override async void Appearing()
        {
            try
            {
                IsBusy = true;

                var product = await _getProductDetailsRequest.Send(new GetProductDetailsInput() { Id = _chooseProductService.ProductSelectedId.ToString() });
               
                if (product.Success)
                {
                    Product = product;
                }
                else
                {
                    _dialogService.ShowConfirmDialog(product.ErrorMessage ?? "Ocorreu um erro. Tente novamente", "Ok", ConfirmAction);
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
