using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.Core.Services.Implementations.Clients
{
    public class ClientsManagerService : IClientsManagerService
    {
        private List<Client> _clientsList;
        public List<Client> ClientsList => _clientsList;

        public ClientsManagerService()
        {
        }

        public void SetClients(List<Client> clientsList)
        {
            _clientsList = clientsList;
        }

        public void SetNewOrder(int clientId, ExtraOrder extraOrder)
        {
            Client client = ClientsList.Find(cli => cli.Id == clientId);
            client.SetNewOrder(extraOrder);
        }

        public void SetNewRegist(int clientId, Regist detail)
        {
            Client client = ClientsList.Find(cli => cli.Id == clientId);
            client.SetNewRegist(detail);
        }

        //USED in APP
        public Regist SetPayment(Client client, DateTime dateSelected, bool payExtra)
        {
            var regist = new Regist(
                DateTime.Today,
                "Pagamento realizado até dia " + dateSelected.ToString("dd/MM/yyyy"),
                client.Id,
                DetailTypeEnum.Payment
                );
            client.SetPaymentDate(dateSelected, payExtra);
            client.SetNewRegist(regist);

            return regist;
        }

        public Regist AddExtra(Client client, double extra)
        {
            var regist = new Regist(
                   DateTime.Today,
                   "Adicionado um extra de " + extra.ToString("C"),
                   client.Id,
                   DetailTypeEnum.AddExtra
                   );
            client.AddExtra(extra);
            client.SetNewRegist(regist);
            return regist;
        }
        public ExtraOrder AddNewOrder(Client client, ExtraOrder extraOrder)
        {
            client.SetNewOrder(extraOrder);
            return extraOrder;
        }

        public Regist RemoveExtraOrder(Client client, ExtraOrder order)
        {
            var regist = new Regist(
                      DateTime.Today,
                      "Encomenda cancelada do dia " + order.OrderDay.ToString("dd/MM/yyyy"),
                      client.Id,
                      DetailTypeEnum.CancelOrder
                      );

            client.RemoveOrder(order);
            client.SetNewRegist(regist);
            return regist;
        }

        public Client GetClosestClient(double currentLatitude, double currentLogitude)
        {
            Dictionary<Client, double> distances = new Dictionary<Client, double>();
            foreach (Client client in ClientsList)
            {
                if (client.Address.Coordenadas.Equals("null")) continue;

                double distance = Math.Sqrt(Math.Pow(double.Parse(client.Address.Lat) - currentLatitude, 2) + Math.Pow(double.Parse(client.Address.Lgt) - currentLogitude, 2));
                distances.Add(client, distance);
            }
            double minimumDistance = distances.Min(distance => distance.Value);
            return distances.First(distance => distance.Value == minimumDistance).Key;
        }

        public ExtraOrder HasOrderThisDate(Client client, DateTime dateTime)
        {
            foreach(var extraorder in client.ExtraOrdersList)
            {
                if (extraorder.OrderDay.Date == dateTime.Date)
                    return extraorder;
            }
            return null;
        }
    }
}
