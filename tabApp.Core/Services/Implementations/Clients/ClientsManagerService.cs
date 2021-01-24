using System;
using System.Collections.Generic;
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
    }
}
