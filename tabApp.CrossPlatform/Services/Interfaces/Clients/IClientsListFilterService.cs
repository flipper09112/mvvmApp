using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Clients
{
    public interface IClientsListFilterService
    {
        string FilterString { get; }
        bool HasFilter { get; }

        void SetFilter(string filter);

        void ClearFilter();
        List<Client> FilterClients(List<Client> clientsList);
    }
}
