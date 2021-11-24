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

            ShowHomePage = new MvxCommand(ShowHome, CanShowHome);
        }
        public string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                ShowHomePage.RaiseCanExecuteChanged();
            }
        }

        public string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                ShowHomePage.RaiseCanExecuteChanged();
            }
        }

        private bool CanShowHome()
        {
            return Username == "admin" && Password == "1234!";
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
