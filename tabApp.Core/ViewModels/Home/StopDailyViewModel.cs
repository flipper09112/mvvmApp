using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.Home
{
    public class StopDailyViewModel : BaseViewModel
    {
        private IChooseClientService _chooseClientService;
        private IDialogService _dialogService;
        private IDBManagerService _dBManagerService;
        private IAmmountToPayService _ammountToPayService;
        private IMvxNavigationService _navigationService;

        private DateTypeEnum _dateTypeSelected;
        private DateTime _firstDayIndeterminatedDate;
        private DateTime _firstDayDeterminatedDate;
        private DateTime _lastDayDeterminatedDate;
        private StopTypeEnum _stopType;

        public Client Client => _chooseClientService.ClientSelected;

        public EventHandler GoBack;

        public MvxCommand<DateTypeEnum> ShowCalendarPickerCommand;
        public MvxCommand SaveCommand;
        public MvxCommand<StopTypeEnum> SelectTypeCommand;

        public StopDailyViewModel(IChooseClientService chooseClientService, 
                                  IDialogService dialogService,
                                  IDBManagerService dBManagerService,
                                  IAmmountToPayService ammountToPayService,
                                  IMvxNavigationService navigationService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _dBManagerService = dBManagerService;
            _ammountToPayService = ammountToPayService;
            _navigationService = navigationService;

            ShowCalendarPickerCommand = new MvxCommand<DateTypeEnum>(ShowCalendarPicker);
            SaveCommand = new MvxCommand(Save, CanSave);
            SelectTypeCommand = new MvxCommand<StopTypeEnum>(SelectType);
        }

        private void SelectType(StopTypeEnum obj)
        {
            StopType = obj;
        }

        private bool CanSave()
        {
            if (StopType != StopTypeEnum.None) {
                if (StopType == StopTypeEnum.Determinated && FirstDayDeterminatedDate.Date == LastDayDeterminatedDate.Date)
                    return false;
                return true;
            }
            return false;
        }

        private void Save()
        {
            IsBusy = true;
            if(StopType == StopTypeEnum.Determinated)
            {
                SetupClientDeterminatedStop();
                GoBack?.Invoke(null, null);
            } else
            {
                SetupClientIndeterminatedStop();
                GoBack?.Invoke(null, null);
            }
            IsBusy = false;
        }


        public StopTypeEnum StopType
        {
            get
            {
                return _stopType;
            }
            set
            {
                _stopType = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime FirstDayDeterminatedDate { 
            get
            {
                if(_firstDayDeterminatedDate == DateTime.MinValue)
                {
                    _firstDayDeterminatedDate = DateTime.Today;
                }

                return _firstDayDeterminatedDate;
            }
            set
            {
                _firstDayDeterminatedDate = value;
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(FirstDayDeterminatedDate));
            }
        }
        public DateTime FirstDayIndeterminatedDate
        {
            get
            {
                if (_firstDayIndeterminatedDate == DateTime.MinValue)
                {
                    _firstDayIndeterminatedDate = DateTime.Today;
                }

                return _firstDayIndeterminatedDate;
            }
            set
            {
                _firstDayIndeterminatedDate = value;
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(FirstDayIndeterminatedDate));
            }
        }
        public DateTime LastDayDeterminatedDate
        {
            get
            {
                if (_lastDayDeterminatedDate == DateTime.MinValue)
                {
                    _lastDayDeterminatedDate = DateTime.Today;
                }

                return _lastDayDeterminatedDate;
            }
            set
            {
                _lastDayDeterminatedDate = value;
                SaveCommand.RaiseCanExecuteChanged(); 
                RaisePropertyChanged(nameof(LastDayDeterminatedDate));
            }
        }

        private void ShowCalendarPicker(DateTypeEnum dateTypeEnum)
        {
            _dateTypeSelected = dateTypeEnum;
            _dialogService.ShowDatePickerDialog(SetDate);
        }

        private void SetDate(DateTime obj)
        {
            switch(_dateTypeSelected)
            {
                case DateTypeEnum.FirstDayDeterminated:
                    FirstDayDeterminatedDate = obj;
                    break;
                case DateTypeEnum.FirstDayIndeterminated:
                    FirstDayIndeterminatedDate = obj;
                    break;
                case DateTypeEnum.LastDayDeterminated:
                    LastDayDeterminatedDate = obj;
                    break;
            }
        }

        private void SetupClientIndeterminatedStop()
        {
            double total = _ammountToPayService.CalculateUntilDate(Client, FirstDayIndeterminatedDate.AddDays(-1));

            Client.UpdateActive(false);
            Client.UpdateExtraValueToPay(total);
            Client.ResetDailyOrders();
            Client.UpdatePaymentDate(FirstDayIndeterminatedDate.AddDays(-1));

            _dBManagerService.SaveClient(
                Client,
                new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "O cliente deixou de querer o serviço por tempo indeterminado",
                    ClientId = Client.Id,
                    DetailType = DetailTypeEnum.Inativate
                }
            );
        }
        private void SetupClientDeterminatedStop()
        {
            double total = _ammountToPayService.CalculateBetweenDates(Client, FirstDayDeterminatedDate, LastDayDeterminatedDate);
            Client.AddExtra(-1 * total);
            Client.StartDayStopService = FirstDayDeterminatedDate;
            Client.LastDayStopService = LastDayDeterminatedDate;
            _dBManagerService.SaveClient(Client,
                new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "O cliente deixou de querer o serviço durante " + FirstDayDeterminatedDate.ToString("dd/MM/yyyy") + " até " + LastDayDeterminatedDate.ToString("dd/MM/yyyy"),
                    ClientId = Client.Id,
                    DetailType = DetailTypeEnum.Inativate
                });
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }

    public enum DateTypeEnum
    {
        FirstDayIndeterminated,
        FirstDayDeterminated,
        LastDayDeterminated
    }
    public enum StopTypeEnum
    {
        None,
        Determinated,
        InDeterminated
    }
}
