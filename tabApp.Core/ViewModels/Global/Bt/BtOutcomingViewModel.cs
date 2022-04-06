using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.Global.Bt
{
    public class BtOutcomingViewModel : BaseViewModel
    {
        private IBluetoothService _bluetoothService;
        private IClientsManagerService _clientsManagerService;
        private IDialogService _dialogService;

        public MvxCommand SelectDateCommand;

        public BtOutcomingViewModel(IBluetoothService bluetoothService, 
                                    IClientsManagerService clientsManagerService,
                                    IDialogService dialogService)
        {
            _bluetoothService = bluetoothService;
            _clientsManagerService = clientsManagerService;
            _dialogService = dialogService;

            SelectDateCommand = new MvxCommand(SelectDate);
        }

        private void SelectDate()
        {
            _dialogService.ShowDatePickerDialog(SelectDateAction, false);
        }

        private void SelectDateAction(DateTime date)
        {
            DateSelected = date;
        }

        private DateTime _dateSelected = DateTime.Today;
        public DateTime DateSelected
        {
            get
            {
                return _dateSelected;
            }
            set
            {
                _dateSelected = value;
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public List<Client> ClientList => _clientsManagerService.GetClientsUpdatedToday(DateSelected);

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }

        public void Connect(Action connectedLottie, Action errorLottie, Action finishedLottie, Action incrementProgressiveBar)
        {
            _bluetoothService.InitSynchronizeData(connectedLottie, errorLottie, finishedLottie, incrementProgressiveBar);
        }
    }
}
