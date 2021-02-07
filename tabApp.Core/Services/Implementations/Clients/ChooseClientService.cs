using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;

namespace tabApp.Core.Services.Implementations.DB
{
    public class ChooseClientService : IChooseClientService
    {
        private Client _clientSelected;
        public Client ClientSelected => _clientSelected;

        public DateTime PayTo { get; set; }

        public void SelectClient(Client clientSelected)
        {
            _clientSelected = clientSelected;
        }
    }
}
