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

        public List<Client> ClientsUpdatedToday {
            get
            {
                List<Client> clients = new List<Client>();
                ClientsList.ForEach(item => {
                    if (item.LastChangeDate.Date == DateTime.Today)
                        clients.Add(item);
                });
                return clients;
            }
        }
        public ClientsManagerService()
        {
        }

        public void SetClients(List<Client> clientsList)
        {
            _clientsList = clientsList;

            foreach(Client cli in _clientsList)
            {

                var sortedList = cli.ExtraOrdersList.OrderBy(item => item.OrderDay).ToList();
                cli.ExtraOrdersList = sortedList;
                cli.ExtraOrdersList.Reverse();

                var sortedList2 = cli.DetailsList.OrderBy(item => item.DetailRegistDay).ToList();
                cli.DetailsList = sortedList2;
                cli.DetailsList.Reverse();
            }
        }

        public void SetNewOrder(int clientId, ExtraOrder extraOrder)
        {
            Client client = ClientsList.Find(cli => cli.Id == clientId);
            client?.SetNewOrder(extraOrder);
        }

        public void SetNewRegist(int clientId, Regist detail)
        {
            Client client = ClientsList.Find(cli => cli.Id == clientId);
            client?.SetNewRegist(detail);
        }

        //USED in APP
        public Regist SetPayment(Client client, DateTime dateSelected, bool payExtra, double total)
        {
            var regist = new Regist()
            {
                DetailRegistDay = DateTime.Today,
                Info = "Pagamento realizado até dia " + dateSelected.ToString("dd/MM/yyyy") + "\nToTal: " + total.ToString("C"),
                ClientId = client.Id,
                DetailType = DetailTypeEnum.Payment
            };

            client.SetPaymentDate(dateSelected, payExtra);
            client.SetNewRegist(regist);

            return regist;
        }

        public Regist AddExtra(Client client, double extra)
        {
            var regist = new Regist()
            {
                DetailRegistDay = DateTime.Today,
                Info = "Adicionado um extra de " + extra.ToString("C"),
                ClientId = client.Id,
                DetailType = DetailTypeEnum.AddExtra
            };

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
            var regist = new Regist()
            {
                DetailRegistDay = DateTime.Today,
                Info = "Encomenda cancelada do dia " + order.OrderDay.ToString("dd/MM/yyyy"),
                ClientId = client.Id,
                DetailType = DetailTypeEnum.CancelOrder
            };

            client.RemoveOrder(order);
            client.SetNewRegist(regist);
            return regist;
        }

        public Client GetClosestClient(double currentLatitude, double currentLogitude)
        {
            if (ClientsList == null) return null;

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

        public bool ClientHasExtraOrderThisDay(Client clientSelected, DateTime dateSelected)
        {
            foreach(var order in clientSelected.ExtraOrdersList ?? new List<ExtraOrder>())
            {
                if (order.OrderDay.Date == dateSelected.Date)
                    return true;
            }
            return false;
        }

        public DailyOrder GetTodayDailyOrder(Client client, DayOfWeek day)
        {
            switch(day)
            {
                case DayOfWeek.Monday:
                    return client.SegDailyOrder;
                    break;
                case DayOfWeek.Tuesday:
                    return client.TerDailyOrder;
                    break;
                case DayOfWeek.Wednesday:
                    return client.QuaDailyOrder;
                    break;
                case DayOfWeek.Thursday:
                    return client.QuiDailyOrder;
                    break;
                case DayOfWeek.Friday:
                    return client.SexDailyOrder;
                    break;
                case DayOfWeek.Saturday:
                    return client.SabDailyOrder;
                    break;
                case DayOfWeek.Sunday:
                    return client.DomDailyOrder;
                    break;
                default:
                    return null;
            }
        }

        public void ReplaceClientModel(Client client)
        {
            int count = 0;
            foreach (Client item in _clientsList)
            {
                if (item.Id == client.Id) {

                    _clientsList[count] = client;
                    break;
                }
                count++;
            }
        }

        public List<string> GetAllPaymentsTypes()
        {
            List<string> paymentTypes = new List<string>();

            foreach(Client client in ClientsList)
            {
                if (paymentTypes.Contains(client.PaymentType.ToString())) continue;

                paymentTypes.Add(client.PaymentType.ToString());
            }

            return paymentTypes;
        }

        public int GetNewId()
        {
            int newId = 0;

            foreach (Client client in ClientsList)
            {
                if (client.Id > newId) newId = client.Id;
            }

            return newId + 1;
        }

        public void AddNewClient(Client newClient)
        {
            ClientsList.Add(newClient);
            _clientsList = ClientsList.OrderBy(item => item.Position).ToList<Client>();
        }
    }
}
