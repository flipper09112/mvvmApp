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
        Regist SetPayment(Client client, DateTime dateSelected);
        void SetNewOrder(int clientId, ExtraOrder extraOrder);
        void SetNewRegist(int clientId, Regist detail);
    }
}
