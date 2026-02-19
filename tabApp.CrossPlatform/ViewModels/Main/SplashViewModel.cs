using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Helpers;
using tabApp.Core.ViewModels.Login;

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
            bool hasLogin = await HasLoginAsync();
            if(hasLogin)
                await _navigationService.Navigate<HomeViewModel>();
            else
                await _navigationService.Navigate<LoginViewModel>();
        }

        private async Task<bool> HasLoginAsync()
        {
            string data = await SecureStorageHelper.GetKeyAsync(SecureStorageHelper.HasLoginKey);

            if (data == SecureStorageHelper.HasLoginYesValue)
                return true;

            return false;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
