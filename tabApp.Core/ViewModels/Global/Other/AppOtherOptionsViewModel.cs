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
        public MvxCommand ShowNotificationsCommand;
        public AppOtherOptionsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowReportCommand = new MvxCommand(ShowReport);
            ShowNotificationsCommand = new MvxCommand(ShowNotifications);

            CreateListOptions();
        }

        private async void ShowNotifications()
        {
            //await _navigationService.Navigate<NotificationsDashBoardViewModel>();
        }

        private async void ShowReport()
        {
            await _navigationService.Navigate<ReportViewModel>();
        }

        private void CreateListOptions()
        {
            if (Options == null) Options = new List<Option>();
            Options.Add(new Option(ShowReportCommand, "Report", "ic_pdf"));
            Options.Add(new Option(ShowNotificationsCommand, "Ver notificações", "ic_notification_other"));
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
