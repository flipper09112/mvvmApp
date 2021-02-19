using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.Clients;

namespace tabApp.Core.ViewModels.Global.Bt
{
    public class BtOutcomingViewModel : BaseViewModel
    {
        private IBluetoothService _bluetoothService;
        private IClientsManagerService _clientsManagerService;

        public BtOutcomingViewModel(IBluetoothService bluetoothService, IClientsManagerService clientsManagerService)
        {
            _bluetoothService = bluetoothService;
            _clientsManagerService = clientsManagerService;
        }

        public List<Client> ClientList => _clientsManagerService.ClientsUpdatedToday;

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
