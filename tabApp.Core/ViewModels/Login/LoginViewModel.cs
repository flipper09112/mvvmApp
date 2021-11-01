using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;

namespace tabApp.Core.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;

        public MvxCommand ShowHomePage;

        public LoginViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowHomePage = new MvxCommand(ShowHome);
        }

        private async void ShowHome()
        {
            await SecureStorageHelper.SaveKeyAsync(SecureStorageHelper.HasLoginKey, SecureStorageHelper.HasLoginYesValue);
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
