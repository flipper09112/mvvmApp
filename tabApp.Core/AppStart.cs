using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Main;

namespace tabApp.Core
{
    public class AppStart : MvxAppStart
    {

        private readonly IMvxNavigationService _navigationService;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
            _navigationService = mvxNavigationService;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return _navigationService.Navigate<MainViewModel>();
        }
    }
}
