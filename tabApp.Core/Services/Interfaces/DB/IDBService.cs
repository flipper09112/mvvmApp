using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.DB
{
    public interface IDBService
    {
        Task StartAsync();
        void SaveNewClientData(Client client);
        void SaveNewRegist(Regist regist);
        void SaveNewRegist(ExtraOrder order);
        void RemoveRegist(ExtraOrder obj);
        void SaveAllDocs();
    }
}
