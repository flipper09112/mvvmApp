using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient
{
    public class HomePageViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;

        public MvxCommand ShowHomePageCommand;

        public HomePageViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowHomePageCommand = new MvxCommand(ShowHomePage);
        }

        private async void ShowHomePage()
        {
            await _navigationService.Navigate<HomepageViewModelClient>();
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
