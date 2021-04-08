using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.ViewModels.ClientPage;
using tabApp.Core.ViewModels.ClientPage.OtherOptions;

namespace tabApp.Core.ViewModels
{
    public class OtherOptionsViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IMvxNavigationService _navigationService;

        public EventHandler ShowCalculatorEvent;

        public MvxCommand InsertNewRegistCommand;
        public MvxCommand PrintPageCommand;
        public MvxCommand ShowCalculatorCommand;
        public MvxCommand ChangeDailyOrdersCommand;
        public MvxCommand ShowCreateNoficationPageCommand;

        public OtherOptionsViewModel(IChooseClientService chooseClientService, IMvxNavigationService navigationService)
        {
            _chooseClientService = chooseClientService;
            _navigationService = navigationService;

            InsertNewRegistCommand = new MvxCommand(InsertNewRegist);
            PrintPageCommand = new MvxCommand(PrintPage);
            ShowCalculatorCommand = new MvxCommand(ShowCalculator);
            ChangeDailyOrdersCommand = new MvxCommand(ChangeDailyOrders);
            ShowCreateNoficationPageCommand = new MvxCommand(ShowCreateNoficationPage);

        }

        private async void ShowCreateNoficationPage()
        {
            await _navigationService.Navigate<CreateNoficationViewModel>();
        }

        private async void ChangeDailyOrders()
        {
            await _navigationService.Navigate<ChangeDailyOrderViewModel>();
        }

        private void ShowCalculator()
        {
            ShowCalculatorEvent?.Invoke(null, null);
        }

        private async void PrintPage()
        {
            await _navigationService.Navigate<PrintAccountViewModel>();
        }

        private async void InsertNewRegist()
        {
            await _navigationService.Navigate<AddStoreRegistViewModel>();
        }

        public List<Option> Options
        {
            get
            {
                List<Option> options = new List<Option>();

                options.Add(new Option(PrintPageCommand, "Imprimir Conta", "ic_printer"));
                options.Add(new Option(ShowCalculatorCommand, "Abrir Calculadora", "ic_calculator"));
                options.Add(new Option(ChangeDailyOrdersCommand, "Alterar Quantidade", "ic_change"));
                if(_chooseClientService.ClientSelected.PaymentType == PaymentTypeEnum.Loja)
                    options.Add(new Option(InsertNewRegistCommand, "Inserir despesa do dia", "ic_insert"));

                options.Add(new Option(ShowCreateNoficationPageCommand, "Adicionar notificação", "ic_notification"));

                return options;
            }
        }
        public override void Appearing()
        {
        }
        public override void DisAppearing()
        {
        }
    }
}
