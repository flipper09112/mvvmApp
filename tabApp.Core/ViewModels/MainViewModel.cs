using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
        }

        public MvxAsyncCommand ShowHomePage { get; private set; }
    }
}
