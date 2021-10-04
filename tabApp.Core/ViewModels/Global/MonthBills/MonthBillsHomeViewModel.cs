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
        private IAmmountToPayService _ammountToPayService;

        public MvxCommand PrintCommand;
        public MonthBillsHomeViewModel(IClientsManagerService clientsManagerService,
                                       IBluetoothService bluetoothService,
                                       IAmmountToPayService ammountToPayService)
        {
            _clientsManagerService = clientsManagerService;
            _bluetoothService = bluetoothService;
            _ammountToPayService = ammountToPayService;

            PrintCommand = new MvxCommand(DoPrint);
        }

        private async void DoPrint()
        {
            _bluetoothService.Connect(PairedDevicesSelected);

            int count = 0;
            foreach(var cli in MonthClients)
            {
                count++;
                if(cli.Selected)
                    await PrintBill(cli.Data);
            }

            _bluetoothService.DisConnect(PairedDevicesSelected);
        }

        private async Task PrintBill(Client client)
        {
            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.CenterAlign(), PairedDevicesSelected);
            await PrintLogo.Invoke();

            await _bluetoothService.SendDataAsync("Fontao, Ponte de Lima\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Contacto: 964690528\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("------------------------------------------------\n\n", PairedDevicesSelected);

            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.LeftAlign(), PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Nome cliente: " + client.Name.RemoveDiacritics() + "\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Cliente id: " + client.Id  + "\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Morada: " + client.Address.AddressDesc.RemoveDiacritics() + "\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("------------------------------------------------\n\n\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);

            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.CenterAlign(), PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(ThermalPrinterFormatsHelper.DoubleHeightAndWidth(), PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Conta respetiva ate dia\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(GetLastDayOfMouth(), PairedDevicesSelected);

            await PrintPrice.Invoke(_ammountToPayService.Calculate(client, GetLastDayOfMouthDate()));

            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);

            await _bluetoothService.SendDataAsync(new ThermalPrinterFormatsHelper().Get(), PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("------------------------------------------------\n\n\n\n", PairedDevicesSelected);

            await PrintBarCode.Invoke();

            await _bluetoothService.SendDataAsync("------------------------------------------------\n\n\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Agradecemos a sua preferencia\n\n", PairedDevicesSelected);
            await _bluetoothService.SendDataAsync("Obrigado", PairedDevicesSelected);

            // Finish print job with some spaces (line feeds) at the bottom.
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);
            await _bluetoothService.SendDataAsync(LINE_FEED, PairedDevicesSelected);

            await CutPaperAsync();

            await _bluetoothService.SendDataAsync(new ThermalPrinterFormatsHelper().Get(), PairedDevicesSelected);
        }

        private string GetLastDayOfMouth()
        {
            DateTime date;
            if(DateTime.Today.Day < 15 && DateTime.Today.Day >= 1)
            {
                date = DateTime.Today.AddMonths(-1);
            }
            else
            {
                date = DateTime.Today;
            }
            
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).ToString("dd MMMM yyyy\n\n\n\n");
        }

        private DateTime GetLastDayOfMouthDate()
        {
            DateTime date;
            if (DateTime.Today.Day < 15 && DateTime.Today.Day >= 1)
            {
                date = DateTime.Today.AddMonths(-1);
            }
            else
            {
                date = DateTime.Today;
            }

            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static byte[] LINE_FEED = new byte[] { 0x0A };
        private static char ESC_CHAR = (char)0x1B;

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
        public Func<Task<bool>> PrintBarCode { get; set; }
        public Func<double, Task<bool>> PrintPrice { get; set; }

        public override void Appearing()
        {
            List<Selectable<Client>> list = new List<Selectable<Client>>();
            var clients = _clientsManagerService.ClientsList.Where(cli => cli.PaymentType == PaymentTypeEnum.Mensal).ToList();
            clients = clients.Where(cli => cli.Active).ToList();

            clients.ForEach(item => list.Add(new Selectable<Client>(item)));
            MonthClients = list;

            var devices = _bluetoothService.GetPairedDevices() ?? new List<string>() { "Sem dispositivos" };
            int index = devices.IndexOf("Printer001");

            if(index != -1)
            {
                var item = devices[index];
                var item2 = devices[0];

                devices[0] = item;
                devices[index] = item2;
            }

            PairedDevices = devices;
        }

        public override void DisAppearing()
        {
        }
    }
}
