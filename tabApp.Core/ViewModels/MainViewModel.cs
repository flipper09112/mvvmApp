using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IDBService _dbService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IClientsListFilterService _clientsListFilterService;

        public MvxCommand<(double lat, double lgt)> SetClosestClientCommand { get; private set; }
        public MvxAsyncCommand ShowHomePage { get; private set; }
        public MvxCommand<string> SetFilterCommand { get; set; }

        public EventHandler UpdateUiHomePage;

        public MainViewModel(IDBService dbService, IMvxNavigationService navigationService,
                             IChooseClientService chooseClientService, IClientsManagerService clientsManagerService,
                             IClientsListFilterService clientsListFilterService)
        {
            _navigationService = navigationService;
            _dbService = dbService;
            _chooseClientService = chooseClientService;
            _clientsManagerService = clientsManagerService;
            _clientsListFilterService = clientsListFilterService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
            SetClosestClientCommand = new MvxCommand<(double lat, double lgt)>(ShowClosestClient);
            SetFilterCommand = new MvxCommand<string>(SetFilter);
        }

        private void SetFilter(string obj)
        {
            _clientsListFilterService.SetFilter(obj);
            UpdateUiHomePage?.Invoke(null, null);
        }

        private async void ShowClosestClient((double lat, double lgt) coord)
        {
            IsBusy = true;
            Client client = _clientsManagerService.GetClosestClient(coord.lat, coord.lgt);
            _chooseClientService.SelectClient(client);
            await _navigationService.Navigate<ClientPageViewModel>();
            IsBusy = false;
        }

        public override async void Appearing()
        {
            IsBusy = true;
            await _dbService.StartAsync();
            UpdateUiHomePage?.Invoke(null, null);
            IsBusy = false;
        }
        public override void DisAppearing()
        {
        }
    }
}
