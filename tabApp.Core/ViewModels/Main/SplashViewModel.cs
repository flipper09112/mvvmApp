using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.ViewModels.Main
{
    public class SplashViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;

        public MvxCommand ShowHomePage;

        public SplashViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowHomePage = new MvxCommand(ShowHome);
        }

        private async void ShowHome()
        {
            await _navigationService.Navigate<HomeViewModel>();
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
