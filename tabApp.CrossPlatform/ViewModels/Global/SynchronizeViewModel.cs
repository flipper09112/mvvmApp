using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.ViewModels.Global.Bt;

namespace tabApp.Core.ViewModels.Global
{
    public class SynchronizeViewModel : BaseViewModel
    {
        private IBluetoothService _bluetoothService;
        private IMvxNavigationService _navigationService;

        public MvxCommand IncomingCommand;
        public MvxCommand OutcomingCommand;

        public SynchronizeViewModel(IBluetoothService bluetoothService, IMvxNavigationService navigationService)
        {
            _bluetoothService = bluetoothService;
            _navigationService = navigationService;

            IncomingCommand = new MvxCommand(Incoming);
            OutcomingCommand = new MvxCommand(Outcoming);
        }

        private async void Outcoming()
        {
            await _navigationService.Navigate<BtOutcomingViewModel>();
        }

        private async void Incoming()
        {
            await _navigationService.Navigate<BtIncomingViewModel>();
        }

        public List<string> PairedDevices => _bluetoothService.GetPairedDevices();

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
