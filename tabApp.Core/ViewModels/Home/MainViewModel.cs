using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Interfaces.Timer;
using tabApp.Core.ViewModels.Global;
using tabApp.Core.ViewModels.Global.Other;

namespace tabApp.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IClientsListFilterService _clientsListFilterService;
        private readonly IInativityTimerService _inativityTimerService;
        private readonly IDataBaseManagerService _dataBaseService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IDBService _dbService;

        public MvxCommand<(double lat, double lgt)> SetClosestClientCommand { get; private set; }
        public MvxAsyncCommand ShowHomePage { get; private set; }
        public MvxCommand ShowGlobalOrderPageCommand { get; private set; }
        public MvxCommand ShowPriceTableCommand { get; private set; }
        public MvxCommand SyncronizeCommand { get; private set; }
        public MvxCommand OtherOptionsCommand { get; private set; }
        public MvxCommand<string> SetFilterCommand { get; set; }

        public EventHandler UpdateUiHomePage;

        private bool _alreadyStarted;

        public MainViewModel(IMvxNavigationService navigationService,
                             IChooseClientService chooseClientService, IClientsManagerService clientsManagerService,
                             IClientsListFilterService clientsListFilterService, IInativityTimerService inativityTimerService,
                             IDataBaseManagerService clientsDataBaseService, IProductsManagerService productsManagerService,
                             IDBService dbService)
        {
            _navigationService = navigationService;
            _chooseClientService = chooseClientService;
            _clientsManagerService = clientsManagerService;
            _clientsListFilterService = clientsListFilterService;
            _inativityTimerService = inativityTimerService;
            _dataBaseService = clientsDataBaseService;
            _productsManagerService = productsManagerService;
            _dbService = dbService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
            SetClosestClientCommand = new MvxCommand<(double lat, double lgt)>(ShowClosestClient);
            SetFilterCommand = new MvxCommand<string>(SetFilter);
            ShowGlobalOrderPageCommand = new MvxCommand(ShowGlobalOrderPage);
            ShowPriceTableCommand = new MvxCommand(ShowPriceTable);
            SyncronizeCommand = new MvxCommand(Syncronize);
            OtherOptionsCommand = new MvxCommand(OtherOptionsCommandAction);
        }

        private async void OtherOptionsCommandAction()
        {
            await _navigationService.Navigate<AppOtherOptionsViewModel>();
        }

        private async void Syncronize()
        {
            await _navigationService.Navigate<SynchronizeViewModel>();
        }

        private async void ShowPriceTable()
        {
            await _navigationService.Navigate<PriceTableViewModel>();
        }

        private async void ShowGlobalOrderPage()
        {
            await _navigationService.Navigate<GlobalOrderViewModel>();
        }

        private void SetFilter(string obj)
        {
            _clientsListFilterService.SetFilter(obj);
            UpdateUiHomePage?.Invoke(null, null);
        }

        public void CloseDBConnection()
        {
            _dataBaseService.Database.Dispose();
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
            if (_alreadyStarted || _clientsManagerService?.ClientsList?.Count > 0)
                return;
            _alreadyStarted = true;
            IsBusy = true;

            //---------DB init-------------
           // await _dbService.StartAsync();
            //Here can write xls to sqllite
           // _dataBaseService.InsertAllDataFromXls(_clientsManagerService.ClientsList, _productsManagerService.ProductsList);

            await _dataBaseService.LoadDataBase();
            //end DB init

            UpdateUiHomePage?.Invoke(null, null);
            IsBusy = false;
        }
        public override void DisAppearing()
        {
        }

        public void StarCounting()
        {
            _inativityTimerService.Start();
        }
        public void StopCounting()
        {
            _inativityTimerService.Stop();
        }

        public void RestartSwatch()
        {
            _inativityTimerService.Restart();
        }
    }
}
