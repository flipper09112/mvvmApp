using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.Core.ViewModels.Global.Bt
{
    public class BtIncomingViewModel : BaseViewModel
    {
        private IBluetoothService _bluetoothService;
        private IDataBaseManagerService _dataBaseManagerService;

        public BtIncomingViewModel(IBluetoothService bluetoothService, 
                                   IDataBaseManagerService dataBaseManagerService)
        {
            _bluetoothService = bluetoothService;
            _dataBaseManagerService = dataBaseManagerService;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }

        public void StartServer(Action disableLottie, Action errorLottie, Action finishedLottie)
        {
            _bluetoothService.StartServerSocket(disableLottie, errorLottie, finishedLottie, ReceiveNewClientsData);
        }

        public void ReceiveNewClientsData(List<Client> newClients)
        {
            foreach(Client client in newClients)
            {
                _dataBaseManagerService.UpdateClientFromBluetooth(client);
            }
        }

        public void StopServer()
        {
            _bluetoothService.StopServerSocket();
        }
    }
}
