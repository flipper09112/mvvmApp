using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.Global.ChangePrices
{
    public class ChangePricesViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private IAmmountToPayService _ammountToPayService;
        private IClientsManagerService _clientsManagerService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IMvxNavigationService _navigationService;

        public MvxCommand DateClickCommand;
        public MvxCommand ContinueCommand;

        public EventHandler GoBack2Times;

        private DateTime _dateSelected;
        public DateTime DateSelected
        {
            get => _dateSelected;
            set
            {
                _dateSelected = value;
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public ChangePricesViewModel(IDialogService dialogService,
                                     IAmmountToPayService ammountToPayService,
                                     IClientsManagerService clientsManagerService,
                                     IDataBaseManagerService dataBaseManagerService,
                                     IMvxNavigationService navigationService)
        {
            _dialogService = dialogService;
            _ammountToPayService = ammountToPayService;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;
            _navigationService = navigationService;

            DateClickCommand = new MvxCommand(DateClick);
            ContinueCommand = new MvxCommand(Continue);

            _dateSelected = DateTime.Today;
        }

        private void Continue()
        {
            IsBusy = true;
            foreach(Client client in _clientsManagerService.ClientsList)
            {
                if (client.PaymentType == PaymentTypeEnum.Loja || client.PaymentType == PaymentTypeEnum.JuntaDiasLoja)
                    continue;

                var value = UpdateClient(client);

                string toRegist = "Ultio dia com os valores antigos de precos dos produtos. (" + DateSelected.ToString("dd/MM/yyyy") + ")\n" +
                    "Fixado valor de extra de " + value.ToString("C");
                Regist regist = new Regist()
                { 
                    DetailRegistDay = DateTime.Today,
                    DetailType = DetailTypeEnum.ChangePrices,
                    Info = toRegist
                };
                _dataBaseManagerService.SaveClient(client, regist);
            }
            IsBusy = false;
            GoBack2Times?.Invoke(null, null);
        }

        private double UpdateClient(Client client)
        {
            double extra = _ammountToPayService.CalculateUntilDate(client, DateSelected.AddDays(-1));
            client.UpdatePaymentDate(DateSelected.AddDays(-1));
            client.UpdateExtraValueToPay(extra);

            return extra;
        }

        private void DateClick()
        {
            _dialogService.ShowDatePickerDialog(DateSelectedAction, true, DateTime.Today);
        }

        private void DateSelectedAction(DateTime obj)
        {
            DateSelected = obj;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
