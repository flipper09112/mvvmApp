using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IDBService _dbService;

        public MainViewModel(IDBService dbService, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _dbService = dbService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
        }

        public MvxAsyncCommand ShowHomePage { get; private set; }

        public override async void AppearingAsync()
        {
            await _dbService.StartAsync();
        }
    }
}
