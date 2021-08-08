using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Models.Notifications;

namespace tabApp.Core.Services.Implementations.DB
{
    public interface IDataBaseManagerService
    {
        SQLiteConnection Database { get; set; }

        List<Client> GetClients();
        List<Product> GetProducts();
        List<Notification> GetNotifications();

        void InsertClient(Client contact);
        void InsertAllProducts(List<Product> products);
        void InsertAllDataFromXls(List<Client> clientsList, List<Product> productsList);

        Task LoadDataBase();
        void SaveClient(Client client, string toRegist);
        void UpdateClientFromBluetooth(Client client);
        void SaveClient(Client client, Regist regist);
        void SaveClient(Client clientSelected, ExtraOrder order);
        void SaveProduct(Product productSelected);
        void UpdateOrder(ExtraOrder regist);
        void SaveAllDocs();
        void RemoveClient(Client client);
        void RemoveClient(int id);
        void RemoveExtraOrder(Client client, ExtraOrder obj, Regist regist);
        void InsertNotification(Notification notification);
        void InsertNewProduct(Product product);
        void InsertClient(Client newClient, Regist regist);
        void InsertGlobalOrderRegist(GlobalOrderRegist globalOrderRegist);
    }
}