using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.ViewModels;

namespace tabApp.Core
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IClientsManagerService _clientsManagerService;


        public HomeViewModel(IClientsManagerService clientsManagerService)
        {
            _clientsManagerService = clientsManagerService;
        }

        public List<Client> ClientsList
        {
            get
            {
                return _clientsManagerService.ClientsList;
            }
        }

        public override void AppearingAsync()
        {
        }
    }
}
