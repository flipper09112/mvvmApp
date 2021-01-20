using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;

namespace tabApp.Core.Services.Implementations.Clients
{
    public class ClientsManagerService : IClientsManagerService
    {
        private List<Client> _clientsList;
        public List<Client> ClientsList => _clientsList;

        public void SetClients(List<Client> clientsList)
        {
            _clientsList = clientsList;
        }
    }
}
