﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.ClientPage.OtherOptions
{
    public class CreateNoficationViewModel : BaseViewModel
    {
        private IChooseClientService _chooseClientService;
        private IMvxNavigationService _navigationService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IDialogService _dialogService;

        public Client Client => _chooseClientService.ClientSelected;
        public List<NotificationType> Options { get; set; }

        public EventHandler GoBack;
        public MvxCommand DailyClientDontPayNotificationCommand;
        public MvxCommand DailyClientDontPaySelecteDateNotificationCommand;

        public CreateNoficationViewModel(IChooseClientService chooseClientService,
                                         IMvxNavigationService navigationService,
                                         IDataBaseManagerService dataBaseManagerService,
                                         IDialogService dialogService)
        {
            _chooseClientService = chooseClientService;
            _navigationService = navigationService;
            _dataBaseManagerService = dataBaseManagerService;
            _dialogService = dialogService;

            DailyClientDontPayNotificationCommand = new MvxCommand(DailyClientDontPayNotification);
            DailyClientDontPaySelecteDateNotificationCommand = new MvxCommand(DailyClientDontPaySelecteDateNotification);


            CreateOptionsList();
        }



        #region Command Functions
        private void DailyClientDontPaySelecteDateNotification()
        {
            _dialogService.ShowDatePickerDialog(SelectDate, true, DateTime.Today.AddDays(1));
        }

        private void SelectDate(DateTime date)
        {
            Notification notification = new Notification()
            {
                Info = "O Cliente não pagou no dia " + DateTime.Today.ToString("dd/MM/yyyy"),
                ClientId = Client.Id,
                NotificationType = NotificationTypeEnum.DontPay,
                AlertDay = date,
                Latitude = Client.Address.HasCoord ? Client.Address.Lat : string.Empty,
                Longitude = Client.Address.HasCoord ? Client.Address.Lgt : string.Empty,
            };

            _dataBaseManagerService.InsertNotification(notification);

            GoBack?.Invoke(null, null);
            GoBack?.Invoke(null, null);
        }

        private void DailyClientDontPayNotification()
        {
            Notification notification = new Notification() {
                Info = "O Cliente não pagou no dia " + DateTime.Today.ToString("dd/MM/yyyy"),
                ClientId = Client.Id,
                NotificationType = NotificationTypeEnum.DontPay,
                AlertDay = DateTime.Today.AddDays(1),
                Latitude = Client.Address.HasCoord ? Client.Address.Lat : string.Empty,
                Longitude = Client.Address.HasCoord ? Client.Address.Lgt : string.Empty,
            };

            _dataBaseManagerService.InsertNotification(notification);

            GoBack?.Invoke(null, null);
            GoBack?.Invoke(null, null);
        }
        #endregion

        private void CreateOptionsList()
        {
            Options = new List<NotificationType>();

            if(Client.PaymentType == PaymentTypeEnum.Diario)
            {
                Options.Add(new NotificationType()
                {
                    Name = "Não pagou\n(Cliente Diário)",
                    ImageName = "ic_no_money",
                    Action = DailyClientDontPayNotificationCommand
                });

                Options.Add(new NotificationType()
                {
                    Name = "[Escolher dia]Não pagou\n(Cliente Diário)",
                    ImageName = "ic_no_money",
                    Action = DailyClientDontPaySelecteDateNotificationCommand
                });
            }
            
            Options.Add(new NotificationType()
            {
                Name = "Outra",
                ImageName = "ic_notification_other",
                Action = null
            });
        }


        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
