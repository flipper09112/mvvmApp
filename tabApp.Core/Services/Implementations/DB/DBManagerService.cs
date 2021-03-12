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

        public void RemoveClient(Client client)
        {
            _dBService.RemoveClient(client);
        }

        public void SaveClient(Models.Client client, string toRegist)
        {
            _dBService.SaveClientData(client);

            Models.Regist regist = new Regist()
            {
                DetailRegistDay = DateTime.Today,
                Info = toRegist,
                ClientId = client.Id,
                DetailType = DetailTypeEnum.Edit
            };
            client.SetNewRegist(regist);
            _dBService.SaveNewRegist(regist);
        }

        public void SaveClient(Client client, Regist regist)
        {
            _dBService.SaveClientData(client); 
            client.SetNewRegist(regist);
            _dBService.SaveNewRegist(regist);
        }

        public void UpdateClientFromBluetooth(Client client)
        {
            Client oldClient = _clientsManagerService.ClientsList.Find(item => item.Id == client.Id);

            int diff = 0;
            if ((diff = client.DetailsList.Count - oldClient.DetailsList.Count) != 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    _dBService.SaveNewRegist(client.DetailsList[i]);
                }
            }

            diff = 0;
            if ((diff = client.ExtraOrdersList.Count - oldClient.ExtraOrdersList.Count) != 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    _dBService.SaveNewRegist(client.ExtraOrdersList[i]);
                }
            }

            _clientsManagerService.ReplaceClientModel(client);
            _dBService.SaveClientData(client);
        }
    }
}
