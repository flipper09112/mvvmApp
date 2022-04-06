using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Notifications;

namespace tabApp.Core.ViewModels.EditClient
{
    public class CreateNotificationsViewModel : BaseViewModel
    {
        private IMvxNavigationService _navService;
        private IDialogService _dialogService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IChooseClientService _chooseClientService;
        private INotificationsManagerService _notificationsManagerService;

        public List<PreDefinedAlertItem> PreDefinedAlertItems { get; set; }
        public List<DateTime> ExtraDays { get; set; }
        public PreDefinedAlertItem PreDefinedAlertSelected { get; set; }
        public bool NewDayAdded { get; set; }

        public MvxCommand SaveCommand; 
        public MvxCommand AddDateCommand; 
        public MvxCommand<PreDefinedAlertItem> SelectPreDefinedAlertCommand;

        public EventHandler GoBack2Times;
        public CreateNotificationsViewModel(IMvxNavigationService navService,
                                            IDialogService dialogService,
                                            IDataBaseManagerService dataBaseManagerService,
                                            IChooseClientService chooseClientService,
                                            INotificationsManagerService notificationsManagerService)
        {
            _navService = navService;
            _dialogService = dialogService;
            _dataBaseManagerService = dataBaseManagerService;
            _chooseClientService = chooseClientService;
            _notificationsManagerService = notificationsManagerService;

            ExtraDays = new List<DateTime>();

            SaveCommand = new MvxCommand(Save, CanSave);
            AddDateCommand = new MvxCommand(AddDate);
            SelectPreDefinedAlertCommand = new MvxCommand<PreDefinedAlertItem>(SelectPreDefinedAlert);
        }

        private void AddDate()
        {
            _dialogService.ShowDatePickerDialog(AddDateToList, true);
        }

        private void AddDateToList(DateTime obj)
        {
            NewDayAdded = true;
            ExtraDays.Add(obj);
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void SelectPreDefinedAlert(PreDefinedAlertItem obj)
        {
            if (PreDefinedAlertSelected == obj)
                PreDefinedAlertSelected = null;
            else
                PreDefinedAlertSelected = obj;
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async void Save()
        {
            foreach(DateTime date in ExtraDays)
            {
                Notification notification = new Notification()
                {
                    Info = "O Cliente efetuou alteração de quantidades",
                    ClientId = _chooseClientService.ClientSelected.Id,
                    NotificationType = NotificationTypeEnum.OrderChanged,
                    AlertDay = date,
                    Latitude = _chooseClientService.ClientSelected.Address.HasCoord ? _chooseClientService.ClientSelected.Address.Lat : string.Empty,
                    Longitude = _chooseClientService.ClientSelected.Address.HasCoord ? _chooseClientService.ClientSelected.Address.Lgt : string.Empty,
                };

                _dataBaseManagerService.InsertNotification(notification);
            }

            CreatePredeterminatedAlerts();

            _notificationsManagerService.SetNotifications(_dataBaseManagerService.GetNotifications());

            await _navService.Close(this);
        }

        private void CreatePredeterminatedAlerts()
        {
            switch(PreDefinedAlertSelected.ItemType)
            {
                case PreDefinedAlertItemType.TomorrowAndWeekEnd:
                    for (int i = 1; i < 8; i++)
                    {
                        if (DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Saturday ||
                               DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                        {
                            Notification notification = GetNotification(DateTime.Today.AddDays(i));
                            _dataBaseManagerService.InsertNotification(notification);
                        }
                    }
                    Notification notification2 = GetNotification(DateTime.Today.AddDays(1));
                    _dataBaseManagerService.InsertNotification(notification2);
                    return;
                case PreDefinedAlertItemType.Tomorrow:
                    Notification notification1 = GetNotification(DateTime.Today.AddDays(1));
                    _dataBaseManagerService.InsertNotification(notification1);
                    return;
                case PreDefinedAlertItemType.OneWeek:
                    for (int i = 1; i < 8; i++)
                    {
                        Notification notification = GetNotification(DateTime.Today.AddDays(i));
                        _dataBaseManagerService.InsertNotification(notification);
                    }
                    return;
                case PreDefinedAlertItemType.Weekend:
                    for (int i = 1; i < 8; i++)
                    {
                        if(DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Saturday ||
                            DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                        {
                            Notification notification = GetNotification(DateTime.Today.AddDays(i));
                            _dataBaseManagerService.InsertNotification(notification);
                        }
                    }
                    return;
                default:
                    return;
            }
        }

        private Notification GetNotification(DateTime dateTime)
        {
            return new Notification()
            {
                Info = "O Cliente efetuou alteração de quantidades",
                ClientId = _chooseClientService.ClientSelected.Id,
                NotificationType = NotificationTypeEnum.OrderChanged,
                AlertDay = dateTime,
                Latitude = _chooseClientService.ClientSelected.Address.HasCoord ? _chooseClientService.ClientSelected.Address.Lat : string.Empty,
                Longitude = _chooseClientService.ClientSelected.Address.HasCoord ? _chooseClientService.ClientSelected.Address.Lgt : string.Empty,
            };
        }

        private bool CanSave()
        {
            return PreDefinedAlertSelected != null || ExtraDays?.Count != 0;
        }

        public override void Appearing()
        {
            PreDefinedAlertItems = GetPreDefinedAlertItems();
        }

        private List<PreDefinedAlertItem> GetPreDefinedAlertItems()
        {
            List<PreDefinedAlertItem> items = new List<PreDefinedAlertItem>();

            items.Add(new PreDefinedAlertItem()
            {
                ItemName = "Amanha",
                ItemType = PreDefinedAlertItemType.Tomorrow
            });

            items.Add(new PreDefinedAlertItem()
            {
                ItemName = "Amanha e Fim de semana",
                ItemType = PreDefinedAlertItemType.TomorrowAndWeekEnd
            });

            items.Add(new PreDefinedAlertItem() { 
                ItemName = "Uma Semana",
                ItemType = PreDefinedAlertItemType.OneWeek
            });

            items.Add(new PreDefinedAlertItem()
            {
                ItemName = "No fim de semana",
                ItemType = PreDefinedAlertItemType.Weekend
            });

            return items;
        }

        public override void DisAppearing()
        {
        }
    }

    public class PreDefinedAlertItem
    {
        public string ItemName { get; set; }
        public PreDefinedAlertItemType ItemType { get; set; }

    }

    public enum PreDefinedAlertItemType
    {
        OneWeek,
        Weekend,
        TomorrowAndWeekEnd,
        Tomorrow
    }
}
