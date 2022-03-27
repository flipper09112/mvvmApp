using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Clients
{
    public interface IClientsManagerService
    {
        List<Client> ClientsList { get; }
        List<Client> GetClientsUpdatedToday(DateTime dateSelected);
        string DeliveryId { get; set; }

        void SetClients(List<Client> clientsList, string deliveryId);
        void SetNewOrder(int clientId, ExtraOrder extraOrder);
        void SetNewRegist(int clientId, Regist detail);
        Regist SetPayment(Client client, DateTime dateSelected, bool payExtra, double value);
        Regist AddExtra(Client client, double extra);
        ExtraOrder AddNewOrder(Client client, ExtraOrder extraOrder);
        Regist RemoveExtraOrder(Client client, ExtraOrder order);
        Client GetClosestClient(double currentLatitude, double currentLogitude);
        ExtraOrder HasOrderThisDate(Client client, DateTime dateTime);
        bool ClientHasExtraOrderThisDay(Client clientSelected, DateTime dateSelected);
        DailyOrder GetTodayDailyOrder(Client client, DayOfWeek day);
        Client GetClientById(int clientId);
        void ReplaceClientModel(Client client);
        List<string> GetAllPaymentsTypes();
        int GetNewId();
        void AddNewClient(Client newClient);
    }
}
