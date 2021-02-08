using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
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

        public void SaveClient(Models.Client client, string toRegist)
        {
            _dBService.SaveNewClientData(client);
            Models.Regist regist = new Models.Regist(DateTime.Today, toRegist, client.Id, Models.DetailTypeEnum.Edit);
            client.SetNewRegist(regist);
            _dBService.SaveNewRegist(regist);
        }
    }
}
