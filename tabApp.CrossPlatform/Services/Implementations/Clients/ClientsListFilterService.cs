using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;

namespace tabApp.Core.Services.Implementations.Clients
{
    public class ClientsListFilterService : IClientsListFilterService
    {
        private string _filterString = "";
        public string FilterString => _filterString;

        public bool HasFilter => _filterString.Length > 2;

        public void ClearFilter()
        {
            _filterString = "";
        }

        public List<Client> FilterClients(List<Client> clientsList)
        {
            List<Client> clients = new List<Client>();

            foreach(var client in clientsList)
            {
                if(client.Name.ToLower().Contains(_filterString.ToLower())
                    || client.Address.AddressDesc.ToLower().Contains(_filterString.ToLower())
                    || client.Id.ToString().ToLower().Contains(_filterString.ToLower())
                    )
                {
                    clients.Add(client);
                }
            }
            return clients;
        }

        public void SetFilter(string filter)
        {
            _filterString = filter;
        }
    }
}
