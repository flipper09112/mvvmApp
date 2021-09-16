using Android.Graphics;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Models.General;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.Clients;

namespace tabApp.Core.ViewModels.Global.MonthBills
{
    public class MonthBillsHomeViewModel : BaseViewModel
    {
        private IClientsManagerService _clientsManagerService;
        private IBluetoothService _bluetoothService;

        public MvxCommand PrintCommand;
        public MonthBillsHomeViewModel(IClientsManagerService clientsManagerService,
                                       IBluetoothService bluetoothService)
        {
            _clientsManagerService = clientsManagerService;
            _bluetoothService = bluetoothService;

            PrintCommand = new MvxCommand(DoPrint);
        }

        private async void DoPrint()
        {
            IsBusy = true;

            string names = "";
            MonthClients.ForEach(cli => {

                names += cli.Data.Name + "\n";
            });

            _bluetoothService.Connect(PairedDevicesSelected);


            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.CenterAlign(), PairedDevicesSelected);
            await PrintLogo.Invoke();

            await _bluetoothService.SendDataAsync(EMPHASIZED_MODE_OFF, PairedDevicesSelected); 
            byte size = (byte)(12 * FONT_POINT);
            _bluetoothService.SendData(size, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.CenterAlign(), PairedDevicesSelected);

            await _bluetoothService.SendDataAsync(names, PairedDevicesSelected);

            // Finish print job with some spaces (line feeds) at the bottom.
            // TODO this could be fixed with some printer configuration.
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            _bluetoothService.DisConnect(PairedDevicesSelected);

            IsBusy = false;
        }

        private static byte[] LINE_FEED = new byte[] { 0x0A };
        private static byte FONT_POINT = 0x11;
        private static char ESC_CHAR = (char)0x1B;
        private static byte[] EMPHASIZED_MODE_OFF = new byte[] { (byte)ESC_CHAR, 0x45, 0x00 };

        public IBluetoothService BluetoothService => _bluetoothService;

        private async Task CutPaperAsync()
        {
            string GS = Convert.ToString((char)29);
            string ESC = Convert.ToString((char)27);
            string COMMAND = "";
            COMMAND = ESC + "@";
            COMMAND += GS + "V" + (char)48;

            await _bluetoothService.SendDataAsync(COMMAND, PairedDevicesSelected);
        }

        public List<Selectable<Client>> MonthClients { get; private set; }
        public List<string> PairedDevices { get; private set; }
        public string PairedDevicesSelected { get; set; }
        public Func<Task<bool>> PrintLogo { get; set; }

        public override void Appearing()
        {
            List<Selectable<Client>> list = new List<Selectable<Client>>();
            var clients = _clientsManagerService.ClientsList.Where(cli => cli.PaymentType == PaymentTypeEnum.Mensal).ToList();
            clients = clients.Where(cli => cli.Active).ToList();

            clients.ForEach(item => list.Add(new Selectable<Client>(item)));
            MonthClients = list;

            PairedDevices = _bluetoothService.GetPairedDevices() ?? new List<string>() { "Sem dispositivos" };
        }

        public override void DisAppearing()
        {
        }
    }
}
