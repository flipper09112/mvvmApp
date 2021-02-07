using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.ClientPage.OtherOptions
{
    public class PrintAccountViewModel : BaseViewModel
    {
        private readonly IGetDataToPrintService _getDataToPrintService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IDialogService _dialogService;
        private readonly IBluetoothService _bluetoothService;

        public List<PrintPreview> _printPreviewList = new List<PrintPreview>();

        public MvxCommand<PrintPreview> SetSelectedCommand;
        public MvxCommand PrintCommand;
        public MvxCommand ShowDatePickerCommand;

        public PrintAccountViewModel(IGetDataToPrintService getDataToPrintService, IChooseClientService chooseClientService,
                                     IDialogService dialogService, IBluetoothService bluetoothService)
        {
            _getDataToPrintService = getDataToPrintService;
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _bluetoothService = bluetoothService;

            SetSelectedCommand = new MvxCommand<PrintPreview>(SetSelected);
            PrintCommand = new MvxCommand(Print, CanPrint);
            ShowDatePickerCommand = new MvxCommand(ShowDatePicker);
        }

        private void ShowDatePicker()
        {
            _dialogService.ShowDatePickerDialog(SetDate);
        }

        private void SetDate(DateTime obj)
        {
            DateSelected = obj;
        }

        private bool CanPrint()
        {
            if (_printPreviewList.Find(item => item.Selected) != null)
                return true;

            return false;
        }

        private void Print()
        {
            var preview = _printPreviewList.Find(item => item.Selected);
            _bluetoothService.SendData(preview.Preview);
        }

        private void SetSelected(PrintPreview obj)
        {
            _printPreviewList.ForEach(item =>
            {
                if (item != obj)
                    item.Selected = false;
            });
            RaisePropertyChanged(nameof(PrintPreviewList));
            PrintCommand.RaiseCanExecuteChanged();
        }

        private DateTime? _dateSelected;
        public DateTime DateSelected
        {
            get
            {
                return _dateSelected == null ? _chooseClientService.PayTo : (DateTime)_dateSelected;
            }
            set
            {
                _dateSelected = value;
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public List<PrintPreview> PrintPreviewList
        {
            get
            {
                return _printPreviewList;
            }
            set
            {
                _printPreviewList = value;
            }
        }
        private List<string> _pairedDevices;
        public List<string> PairedDevices
        {
            get
            {
                return _pairedDevices;
            }
            set
            {
                _pairedDevices = value; 
                var index = _pairedDevices.FindIndex(x => x.Equals(_bluetoothService.BTDefaultDevice));
                if(index != null)
                {
                    var item = _pairedDevices[index];
                    _pairedDevices[index] = _pairedDevices[0];
                    _pairedDevices[0] = item;
                }
                RaisePropertyChanged(nameof(PairedDevices));
            }
        }

        public override void Appearing()
        {
            PairedDevices = _bluetoothService.GetPairedDevices();
            PrintPreviewList = _getDataToPrintService.GetDataToPrint(_chooseClientService.ClientSelected);

            PrintPreviewList.ForEach(item => item.SetSelectedCommand = SetSelectedCommand);
        }

        public override void DisAppearing()
        {
        }
    }
}
