using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global.Faturation
{
    public class FaturationHomeViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IFaturationService _faturationService;
        private IProductsManagerService _productsManagerService;

        public MvxCommand ShowTransportationDocsPageCommand;
        public MvxCommand ShowFaturationPageCommand;

        public FaturationHomeViewModel(IMvxNavigationService navigationService, 
                                       IFaturationService faturationService,
                                       IProductsManagerService productsManagerService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;
            _productsManagerService = productsManagerService;

            ShowTransportationDocsPageCommand = new MvxCommand(ShowTransportationDocsPage);
            ShowFaturationPageCommand = new MvxCommand(ShowFaturationPage);
        }

        private async void ShowFaturationPage()
        {
            await _navigationService.Navigate<FaturationViewModel>();
        }

        private async void ShowTransportationDocsPage()
        {
            await _navigationService.Navigate<TransportationDocumentsViewModel>();
        }

        public override async void Appearing()
        {
            IsBusy = true;
            /*foreach(var product in _productsManagerService.ProductsList)
            {
                await _faturationService.Products.AddProduct(product);
                await Task.Delay(3000);
            }*/
            IsBusy = false;
        }

        public override void DisAppearing()
        {
        }
    }
}
