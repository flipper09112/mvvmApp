using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.ViewModels.Global.ChangePrices;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class AppOtherOptionsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public List<Option> Options { get; set; }

        public MvxCommand ShowReportCommand;
        public MvxCommand ShowNotificationsCommand; 
        public MvxCommand ShowFinancialsPageCommand;
        public MvxCommand ShowDatabaseManagerPageCommand;
        public MvxCommand ChangePricesPageCommand;
        public AppOtherOptionsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowReportCommand = new MvxCommand(ShowReport);
            ShowNotificationsCommand = new MvxCommand(ShowNotifications);
            ShowFinancialsPageCommand = new MvxCommand(ShowFinancialsPage);
            ShowDatabaseManagerPageCommand = new MvxCommand(ShowDatabaseManagerPage);
            ChangePricesPageCommand = new MvxCommand(ChangePricesPage);

            CreateListOptions();
        }

        private async void ChangePricesPage()
        {
            await _navigationService.Navigate<ChangePricesViewModel>();
        }

        private async void ShowDatabaseManagerPage()
        {
            await _navigationService.Navigate<DatabaseManagerPageViewModel>();
        }

        private async void ShowFinancialsPage()
        {
            await _navigationService.Navigate<HomeFinancialsViewModel>();
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
            Options.Add(new Option(ShowFinancialsPageCommand, "Finanças", "ic_finances"));
            Options.Add(new Option(ShowDatabaseManagerPageCommand, "DataBase Manage", "ic_database"));
            Options.Add(new Option(ChangePricesPageCommand, "Mudar Preços", "ic_change_price")); 
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
