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
        void SetNewOrder(int clientId, ExtraOrder extraOrder);
        void SetNewRegist(int clientId, Regist detail);
        Regist SetPayment(Client client, DateTime dateSelected, bool payExtra);
        Regist AddExtra(Client client, double extra);
        ExtraOrder AddNewOrder(Client client, ExtraOrder extraOrder);
        Regist RemoveExtraOrder(Client client, ExtraOrder order);
    }
}
