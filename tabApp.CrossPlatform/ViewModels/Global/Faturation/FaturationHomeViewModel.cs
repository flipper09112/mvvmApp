using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.Products;
using Xamarin.Essentials;

namespace tabApp.Core.ViewModels.Global.Faturation
{
    public class FaturationHomeViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IFaturationService _faturationService;
        private IProductsManagerService _productsManagerService;

        public MvxCommand ShowTransportationDocsPageCommand;
        public MvxCommand ShowFaturationPageCommand;
        public MvxCommand ShowFaturaLusaCommand;

        public FaturationHomeViewModel(IMvxNavigationService navigationService, 
                                       IFaturationService faturationService,
                                       IProductsManagerService productsManagerService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;
            _productsManagerService = productsManagerService;

            ShowTransportationDocsPageCommand = new MvxCommand(ShowTransportationDocsPage);
            ShowFaturationPageCommand = new MvxCommand(ShowFaturationPage);
            ShowFaturaLusaCommand = new MvxCommand(ShowFaturaLusa);
        }

        private async void ShowFaturaLusa()
        {
            try
            {
                var uri = new Uri("https://facturalusa.pt/app/vistageral");
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide,
                });
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
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
