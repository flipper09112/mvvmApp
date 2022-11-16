using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Faturation;

namespace tabApp.Core.ViewModels.Global.Faturation
{
    public class FaturationHomeViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IFaturationService _faturationService;

        public MvxCommand ShowTransportationDocsPageCommand;

        public FaturationHomeViewModel(IMvxNavigationService navigationService, 
                                       IFaturationService faturationService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;

            ShowTransportationDocsPageCommand = new MvxCommand(ShowTransportationDocsPage);
        }

        private async void ShowTransportationDocsPage()
        {
            await _navigationService.Navigate<TransportationDocumentsViewModel>();
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
