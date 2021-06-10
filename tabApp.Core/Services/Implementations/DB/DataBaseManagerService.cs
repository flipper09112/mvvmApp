using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Enums;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.DB;
using SQLiteNetExtensions.Extensions;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Notifications;

namespace tabApp.Core.Services.Implementations.DB
{
    public class DataBaseManagerService : IDataBaseManagerService
    {
        private readonly string DataBaseName = "MyContacts.db3";

        private IClientsManagerService _clientsManagerService;
        private IProductsManagerService _productsManagerService;
        private IFirebaseService _firebaseService;
        private IFileService _fileService;
        private INotificationsManagerService _notificationsManagerService;

        private SQLiteConnection database;
        static object locker = new object();
        private ISQLiteService _sQLiteService;

        public DataBaseManagerService(ISQLiteService sQLiteService,
                                      IClientsManagerService clientsManagerService,
                                      IProductsManagerService productsManagerService,
                                      IFileService fileService,
                                      IFirebaseService firebaseService,
                                      INotificationsManagerService notificationsManagerService)
        {
            _sQLiteService = sQLiteService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
            _firebaseService = firebaseService;
            _fileService = fileService;
            _notificationsManagerService = notificationsManagerService;
        }

        private void CheckDataBaseCreated(bool deleteTables)
        {
            if (database != null) return;

            try
            {
                database = _sQLiteService.Connection();

                if (deleteTables) 
                {
                    database.DropTable<Address>();
                    database.DropTable<Regist>();
                    database.DropTable<Client>();
                    database.DropTable<DailyOrder>();
                    database.DropTable<Product>();
                    database.DropTable<ReSaleValues>();
                    database.DropTable<DailyOrderDetails>();
                    database.DropTable<ExtraOrder>();
                    database.DropTable<Notification>();
                }
                
                database.CreateTable<Regist>();
                database.CreateTable<Address>();
                database.CreateTable<DailyOrder>();
                database.CreateTable<Client>();
                database.CreateTable<Product>();
                database.CreateTable<ReSaleValues>();
                database.CreateTable<DailyOrderDetails>();
                database.CreateTable<ExtraOrder>();
                database.CreateTable<Notification>();

            } catch (NotSupportedException e) {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task LoadDataBase()
        {
            if (!_fileService.HasFile(DataBaseName))
            {
                _fileService.SaveFile(DataBaseName, await _firebaseService.GetUrlDownload(DataBaseName));
            }

            CheckDataBaseCreated(false);
            _clientsManagerService.SetClients(GetClients().OrderBy(item => item.Position).ToList<Client>());
            _productsManagerService.SetProducts(GetProducts());
            _notificationsManagerService.SetNotifications(GetNotifications());
        }

        public void SaveAllDocs()
        {
            byte[] fileData = _fileService.GetFile(DataBaseName);
            _firebaseService.SaveFile(DataBaseName, fileData);
        }

        #region Inserts

        public void InsertNewProduct(Product product)
        {
            database.InsertAll(product.ReSaleValues);
            database.InsertWithChildren(product);
        }

        public void InsertClient(Client client)
        {
            CheckDataBaseCreated(false);
            database.Insert(client.Address);
            database.InsertAll(client.DetailsList);
            database.Insert(client.DailyOrders);
            database.InsertWithChildren(client);
        }

        public void InsertClient(Client newClient, Regist regist)
        {
            database.Insert(newClient.Address);
            database.InsertAll(newClient.DetailsList);
            database.InsertWithChildren(newClient);

            newClient.SetNewRegist(regist);
            database.Insert(regist);
            database.UpdateWithChildren(newClient);
            database.Update(newClient);
        }

        public void InsertNotification(Notification notification)
        {
            database.CreateTable<Notification>();
            database.Insert(notification);
        }

        public void InsertAllDataFromXls(List<Client> clients, List<Product> products)
        {
            CheckDataBaseCreated(true);
            foreach (Client cliente in clients)
            {
                database.Insert(cliente.Address);
                database.InsertAll(cliente.DetailsList);

                //DailyOrder
                database.InsertAll(cliente.SegDailyOrder.AllItems);
                database.InsertWithChildren(cliente.SegDailyOrder);
                database.InsertAll(cliente.TerDailyOrder.AllItems);
                database.InsertWithChildren(cliente.TerDailyOrder);
                database.InsertAll(cliente.QuaDailyOrder.AllItems);
                database.InsertWithChildren(cliente.QuaDailyOrder);
                database.InsertAll(cliente.QuiDailyOrder.AllItems);
                database.InsertWithChildren(cliente.QuiDailyOrder);
                database.InsertAll(cliente.SexDailyOrder.AllItems);
                database.InsertWithChildren(cliente.SexDailyOrder);
                database.InsertAll(cliente.SabDailyOrder.AllItems);
                database.InsertWithChildren(cliente.SabDailyOrder);
                database.InsertAll(cliente.DomDailyOrder.AllItems);
                database.InsertWithChildren(cliente.DomDailyOrder);

                //Extra orders
                foreach (ExtraOrder extraOrder in cliente.ExtraOrdersList)
                {
                    database.InsertAll(extraOrder.AllItems);
                    database.InsertWithChildren(extraOrder);
                }

                database.InsertWithChildren(cliente);
            }
            InsertAllProducts(products);
        }

        public void InsertAllProducts(List<Product> products)
        {
            foreach (Product product in products)
            {
                database.InsertAll(product.ReSaleValues);
                database.InsertWithChildren(product);
            }
        }

        #endregion

        #region Updates
        public void SaveClient(Client client, string toRegist)
        {
            Regist regist = new Regist()
            {
                DetailRegistDay = DateTime.Today,
                Info = toRegist,
                DetailType = DetailTypeEnum.Edit
            };
            client.SetNewRegist(regist);
            SaveClient(client, regist);
        }

        public void SaveClient(Client client, Regist regist)
        {
            client.LastChangeDate = DateTime.Now;
            CheckDailyOrders(client);

            if(regist != null)
            {
                client.SetNewRegist(regist);
                database.Insert(regist);
                database.UpdateWithChildren(client);
            }

            database.Update(client);
        }

        private void CheckDailyOrders(Client client)
        {
            foreach (DailyOrder dailyOrder in client.DailyOrders)
            {
                if (dailyOrder.Id == 0)
                {
                    database.Insert(dailyOrder);
                    database.UpdateWithChildren(client);
                    database.InsertAll(dailyOrder.AllItems);
                    database.UpdateWithChildren(dailyOrder);
                }
                else
                {
                    List<DailyOrderDetails> itemsToRemove = new List<DailyOrderDetails>();
                    foreach(DailyOrderDetails dailyOrderDetails in dailyOrder.AllItems)
                    {
                        if(dailyOrderDetails.Id == 0 && dailyOrderDetails.Ammount != 0)
                        {
                            database.Insert(dailyOrderDetails);
                            database.UpdateWithChildren(dailyOrder);
                        }
                        else
                        {
                            if (dailyOrderDetails.Ammount == 0)
                            {
                                itemsToRemove.Add(dailyOrderDetails);
                                database.Delete(dailyOrderDetails);
                            }
                            else
                            {
                                database.Update(dailyOrderDetails);
                            }
                        }
                    }
                    if(dailyOrder.AllItems.Count == 0)
                    {
                        var daily = database.GetWithChildren<DailyOrder>(dailyOrder.Id, true);
                        daily.AllItems.ForEach(item => database.Delete(item));
                    }
                    else
                    {
                        dailyOrder.AllItems.RemoveAll(item => itemsToRemove.Contains(item));
                    }
                }
            }
        }

        public void SaveClient(Client client, ExtraOrder regist)
        {
            client.LastChangeDate = DateTime.Now;
            database.Insert(regist);
            database.UpdateWithChildren(client);
            database.InsertAll(regist.AllItems);
            database.UpdateWithChildren(regist);
        }

        public void UpdateClientFromBluetooth(Client client)
        {
            database.Update(client);
            database.Update(client.Address);

           // foreach(ExtraOrder extraOrder in )

            Client oldClient = _clientsManagerService.ClientsList.Find(item => item.Id == client.Id);
            int pos = _clientsManagerService.ClientsList.IndexOf(oldClient);
            _clientsManagerService.ClientsList[pos] = database.GetWithChildren<Client>(client.Id, true);
        }

        #endregion

        #region Gets
        public List<Client> GetClients()
        {
            lock (locker)
            {
                return database.GetAllWithChildren<Client>(recursive: true);
                // return (from c in database.Table<Client>() select c).ToList();
            }
        }

        public List<Product> GetProducts()
        {
            lock (locker)
            {
                return database.GetAllWithChildren<Product>();
                // return (from c in database.Table<Client>() select c).ToList();
            }
        }

        public List<Notification> GetNotifications()
        {
            try
            {
                return database.GetAllWithChildren<Notification>();
            } catch(Exception e)
            {
                return new List<Notification>();
            } 
        }

        #endregion

        #region Delete
        public void RemoveClient(int id)
        {
            database.Delete<Client>(id);
        }

        public void RemoveClient(Client client)
        {
            database.Delete<Client>(client.Id);
        }

        public void RemoveExtraOrder(Client client, ExtraOrder obj, Regist regist)
        {
            obj.AllItems.ForEach(item => database.Delete<DailyOrderDetails>(item.Id));
            database.Delete<ExtraOrder>(obj.Id);
            database.Insert(regist);
            database.UpdateWithChildren(client);
        }


        #endregion
    }
}
