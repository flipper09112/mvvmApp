﻿using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.Home
{
    public class DeleteClientViewModel : BaseViewModel
    {
        private IChooseClientService _chooseClientService;
        private IClientsManagerService _clientsManagerService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IDialogService _dialogService;

        public EventHandler GoBack;
        public MvxCommand DeleteCommand;

        public DeleteClientViewModel(IChooseClientService chooseClientService,
                                     IClientsManagerService clientsManagerService,
                                     IDataBaseManagerService dataBaseManagerService,
                                     IDialogService dialogService)
        {
            _chooseClientService = chooseClientService;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;
            _dialogService = dialogService;

            DeleteCommand = new MvxCommand(Delete);
        }

        private void Delete()
        {
            _dialogService.ShowConfirmDialog("Pretende eliminar de vez este cliente?", "Sim", ConfirmDelete, "Não", null);
        }

        private void ConfirmDelete(object obj)
        {
            IsBusy = true;
            _clientsManagerService.ClientsList.Remove(Client);
            _dataBaseManagerService.RemoveClient(Client);
            GoBack?.Invoke(null, null);
            IsBusy = false;
        }

        public Client Client => _chooseClientService.ClientSelected;

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}