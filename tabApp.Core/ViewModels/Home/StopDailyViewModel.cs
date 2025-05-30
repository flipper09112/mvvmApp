﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Notifications;

namespace tabApp.Core.ViewModels.Home
{
    public class StopDailyViewModel : BaseViewModel
    {
        private IChooseClientService _chooseClientService;
        private IDialogService _dialogService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IAmmountToPayService _ammountToPayService;
        private IMvxNavigationService _navigationService;
        private INotificationsManagerService _notificationsManagerService;

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
                                  IDataBaseManagerService dataBaseManagerService,
                                  IAmmountToPayService ammountToPayService,
                                  IMvxNavigationService navigationService,
                                  INotificationsManagerService notificationsManagerService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _dataBaseManagerService = dataBaseManagerService;
            _ammountToPayService = ammountToPayService;
            _navigationService = navigationService;
            _notificationsManagerService = notificationsManagerService;

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
            _dialogService.ShowDatePickerDialog(SetDate, false);
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

            var notification = new Models.Notifications.Notification() {
                Info = "O cliente não quer mais pão até segunda ordem",
                NotificationType = Models.Notifications.NotificationTypeEnum.StopDailyOrder,
                AlertDay = FirstDayIndeterminatedDate,
                ClientId = Client.Id,
                Latitude = Client.Address.HasCoord ? Client.Address.Lat : string.Empty,
                Longitude = Client.Address.HasCoord ? Client.Address.Lgt : string.Empty
            };

            _dataBaseManagerService.InsertNotification(notification);
            _notificationsManagerService.AllNotifications.Add(notification);

            _dataBaseManagerService.SaveClient(
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
            double total = _ammountToPayService.CalculateBetweenDates(Client, FirstDayDeterminatedDate, LastDayDeterminatedDate, false);
            Client.AddExtra(-1 * total);
            if(FirstDayDeterminatedDate.Date <= DateTime.Today.AddDays(1).Date)
                Client.UpdateActive(false);
            Client.StartDayStopService = FirstDayDeterminatedDate;
            Client.LastDayStopService = LastDayDeterminatedDate;
            _dataBaseManagerService.SaveClient(Client,
                new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "O cliente deixou de querer o serviço durante " + FirstDayDeterminatedDate.ToString("dd/MM/yyyy") + " até " + LastDayDeterminatedDate.ToString("dd/MM/yyyy"),
                    ClientId = Client.Id,
                    DetailType = DetailTypeEnum.Inativate
                });

            AddNotificationsDeterminatedPause();
        }

        private void AddNotificationsDeterminatedPause()
        {
            var notificationStart = new Models.Notifications.Notification()
            {
                Info = "O cliente a partir de hoje (" + FirstDayDeterminatedDate.ToString("dd/MM/yyyy") + ") até dia (" + LastDayDeterminatedDate.ToString("dd/MM/yyyy") + ") não quer pão!",
                NotificationType = Models.Notifications.NotificationTypeEnum.StopDailyOrder,
                AlertDay = FirstDayDeterminatedDate,
                ClientId = Client.Id,
                Latitude = Client.Address.HasCoord ? Client.Address.Lat : string.Empty,
                Longitude = Client.Address.HasCoord ? Client.Address.Lgt : string.Empty
            };

            var notificationEnd = new Models.Notifications.Notification()
            {
                Info = "O cliente a partir de hoje recomeça a usufruir do serviço de entrega!",
                NotificationType = Models.Notifications.NotificationTypeEnum.StopDailyOrder,
                AlertDay = LastDayDeterminatedDate.AddDays(1),
                ClientId = Client.Id,
                Latitude = Client.Address.HasCoord ? Client.Address.Lat : string.Empty,
                Longitude = Client.Address.HasCoord ? Client.Address.Lgt : string.Empty
            };

            _dataBaseManagerService.InsertNotification(notificationStart);
            _dataBaseManagerService.InsertNotification(notificationEnd);
            _notificationsManagerService.AllNotifications.Add(notificationStart);
            _notificationsManagerService.AllNotifications.Add(notificationEnd);
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
