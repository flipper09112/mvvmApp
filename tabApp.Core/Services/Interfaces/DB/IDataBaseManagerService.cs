using System.Collections.Generic;
using System.Threading.Tasks;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.DB
{
    public interface IDataBaseManagerService
    {
        List<Client> GetClients();
        List<Product> GetProducts();

        void InsertClient(Client contact);
        void InsertAllProducts(List<Product> products);
        void InsertAllDataFromXls(List<Client> clientsList, List<Product> productsList);

        Task LoadDataBase();
        void SaveClient(Client client, string toRegist);
        void UpdateClientFromBluetooth(Client client);
        void SaveClient(Client client, Regist regist);
        void SaveClient(Client clientSelected, ExtraOrder order);
        void SaveAllDocs();
        void RemoveClient(Client client);
        void RemoveClient(int id);
        void RemoveExtraOrder(Client client, ExtraOrder obj, Regist regist);
    }
}