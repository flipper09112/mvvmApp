using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.DB
{
    public interface IDBManagerService
    {
        void SaveClient(Models.Client client, string toRegist);
        void SaveClient(Models.Client client, Regist regist);
        void UpdateClientFromBluetooth(Models.Client client);
        void InativateClient(Client client, DateTime firstDayIndeterminatedDate);
    }
}
