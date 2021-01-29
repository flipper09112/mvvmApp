using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.Core.Services.Implementations.DB
{
    public class DBManagerService : IDBManagerService
    {

        private readonly IClientsManagerService _clientsManagerService;
        private readonly IDBService _dBService;

        public DBManagerService(IClientsManagerService clientsManagerService, IDBService dBService)
        {
            _dBService = dBService;
            _clientsManagerService = clientsManagerService;
        }
    }
}
