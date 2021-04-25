using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class AppOtherOptionsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public List<Option> Options { get; set; }

        public MvxCommand ShowReportCommand;
        public AppOtherOptionsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowReportCommand = new MvxCommand(ShowReport);

            CreateListOptions();
        }

        private async void ShowReport()
        {
            await _navigationService.Navigate<ReportViewModel>();
        }

        private void CreateListOptions()
        {
            if (Options == null) Options = new List<Option>();
            Options.Add(new Option(ShowReportCommand, "Report", "ic_pdf"));
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
