using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Bluetooth;

namespace tabApp.Core.ViewModels.Global.Bt
{
    public class BtIncomingViewModel : BaseViewModel
    {
        private IBluetoothService _bluetoothService;

        public BtIncomingViewModel(IBluetoothService bluetoothService)
        {
            _bluetoothService = bluetoothService;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }

        public void StartServer()
        {
            _bluetoothService.StartServerSocket();
        }

        public void StopServer()
        {
            _bluetoothService.StopServerSocket();
        }
    }
}
