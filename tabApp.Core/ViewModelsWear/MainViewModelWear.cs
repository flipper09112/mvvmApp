using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModelsWear.Errors;
using Xamarin.Essentials;

namespace tabApp.Core.ViewModelsWear
{
    public class MainViewModelWear : BaseViewModel
    {
        private bool _alreadyStarted;

        private IClientsManagerService _clientsManagerService;
        private IDataBaseManagerService _dataBaseService;
        private IMvxNavigationService _navigationService;

        public EventHandler UpdatePercentageDownloadEvent;

        public MainViewModelWear(IMvxNavigationService navigationService, 
                                 IClientsManagerService clientsManagerService,
                                 IDataBaseManagerService dataBaseService)
        {
            _clientsManagerService = clientsManagerService;
            _dataBaseService = dataBaseService;
            _navigationService = navigationService;
        }

        public override async void Appearing()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                    await _dataBaseService.LoadDataBase(UpdatePercentageDownloadEvent);
                    await _navigationService.Close(this);
                    await _navigationService.Navigate<NoDatabaseViewModel>();
            }
            else 
            {
                if (_clientsManagerService?.ClientsList?.Count > 0)
                    return;
                else {
                    await _navigationService.Close(this);
                    await _navigationService.Navigate<NoDatabaseViewModel>();
                }
            }

        }

        public override void DisAppearing()
        {
        }
    }
}
