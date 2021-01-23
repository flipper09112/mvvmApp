using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Clients
{
    public interface IClientsManagerService
    {
        List<Client> ClientsList { get; }
        void SetClients(List<Client> clientsList);
        void SetPayment(Client client, DateTime dateSelected);
    }
}
