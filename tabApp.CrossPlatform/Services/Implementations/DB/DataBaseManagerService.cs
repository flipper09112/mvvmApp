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
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Helpers;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.Services.Implementations.DB
{
    public class DataBaseManagerService : IDataBaseManagerService
    {
        public static readonly string DataBaseName = "MyContacts.db3";

        private IClientsManagerService _clientsManagerService;
        private IProductsManagerService _productsManagerService;
        private IFirebaseService _firebaseService;
        private IFileService _fileService;
        private INotificationsManagerService _notificationsManagerService;
        private IGlobalOrdersPastManagerService _globalOrdersPastManagerService;
        private IDeliverysManagerService _deliverysManagerService;
        private IDialogService _dialogService;

        public SQLiteConnection Database { get; set; }
        public bool DBRestored { get; set; }

        static object locker = new object();
        private ISQLiteService _sQLiteService;

        public DataBaseManagerService(ISQLiteService sQLiteService,
                                      IClientsManagerService clientsManagerService,
                                      IProductsManagerService productsManagerService,
                                      IFileService fileService,
                                      IFirebaseService firebaseService,
                                      INotificationsManagerService notificationsManagerService,
                                      IGlobalOrdersPastManagerService globalOrdersPastManagerService,
                                      IDeliverysManagerService deliverysManagerService,
                                      IDialogService dialogService)
        {
            _sQLiteService = sQLiteService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
            _firebaseService = firebaseService;
            _fileService = fileService;
            _notificationsManagerService = notificationsManagerService;
            _globalOrdersPastManagerService = globalOrdersPastManagerService;
            _deliverysManagerService = deliverysManagerService;
            _dialogService = dialogService;
        }

        public void ReloadDB()
        {
            Database = _sQLiteService.Connection();
        }

        private void CheckDataBaseCreated(bool deleteTables)
        {
            if (Database != null) return;

            try
            {
                Database = _sQLiteService.Connection();

                if (deleteTables) 
                {
                    Database.DropTable<Address>();
                    Database.DropTable<Regist>();
                    Database.DropTable<Client>();
                    Database.DropTable<DailyOrder>();
                    Database.DropTable<Product>();
                    Database.DropTable<ReSaleValues>();
                    Database.DropTable<DailyOrderDetails>();
                    Database.DropTable<ExtraOrder>();
                    Database.DropTable<Notification>();
                    Database.DropTable<GlobalOrderRegist>();
                    Database.DropTable<Delivery>();
                    Database.DropTable<PriceChangeDate>();
                }
                
                Database.CreateTable<Regist>();
                Database.CreateTable<Address>();
                Database.CreateTable<DailyOrder>();
                Database.CreateTable<Client>();
                Database.CreateTable<Product>();
                Database.CreateTable<ReSaleValues>();
                Database.CreateTable<DailyOrderDetails>();
                Database.CreateTable<ExtraOrder>();
                Database.CreateTable<Notification>();
                Database.CreateTable<GlobalOrderRegist>();
                Database.CreateTable<Delivery>();
                Database.CreateTable<PriceChangeDate>();

                var tableInfo = Database.GetTableInfo(nameof(Client));
                var columnExists = tableInfo.Any(x => x.Name.Equals(nameof(Client.NIF)));

                if(!columnExists)
                {
                    SQLiteCommand cmd = new SQLiteCommand(Database);
                    cmd.CommandText = "ALTER TABLE Client ADD COLUMN NIF NULL";
                    cmd.ExecuteNonQuery();
                }

            } catch (NotSupportedException e) {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task LoadDataBase(EventHandler updatePercentageDownloadEvent = null)
        {
            if (!_fileService.HasFile(DataBaseName))
            {
                Database = null;
                _fileService.SaveFile(DataBaseName, await _firebaseService.GetUrlDownload(DataBaseName, updatePercentageDownloadEvent));
                SecureStorageHelper.SaveKeyAsync(SecureStorageHelper.DatabaseDateDownloadKey, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            }

            var deliveryId = await SecureStorageHelper.GetKeyAsync(SecureStorageHelper.DeliveryId) ?? "1";
            
            CheckDataBaseCreated(false);
            _clientsManagerService.SetClients(GetClients().OrderBy(item => item.Position).ToList<Client>(), deliveryId);
            _deliverysManagerService.SetDeliverys(GetDelivery());
            _productsManagerService.SetProducts(GetProducts());
            _productsManagerService.SetGetPriceChangeDate(GetPriceChangeDate());
            _notificationsManagerService.SetNotifications(GetNotifications());
            _globalOrdersPastManagerService.SetGlobalOrders(GetGlobalOrderRegists());

            //checks and saves states
            CheckFirstDayAfterStopService();

            //check resales values clients deletes
            CheckReSalesValuesClientsDeleted();
        }

        public void LoadProducts()
        {
            _productsManagerService.SetProducts(GetProducts());
        }

        private void CheckReSalesValuesClientsDeleted()
        {
            var resales = Database.GetAllWithChildren<ReSaleValues>();

            var hasDeletedClient = resales.Any(item => !_clientsManagerService.ClientsList.Any(client => client.Id == item.ClientId));

            if(hasDeletedClient)
            {
                resales.ForEach(item => { 
                    if(!_clientsManagerService.ClientsList.Any(client => client.Id == item.ClientId))
                    {
                        Database.Delete(item);
                    }
                });

                _productsManagerService.SetProducts(GetProducts());
            }
        }

        private void CheckFirstDayAfterStopService()
        {
            _clientsManagerService.ClientsList.ForEach(client => { 
                try
                {
                    if(client.LastDayStopService?.Date <= DateTime.Today.AddDays(1).Date)
                    {
                        if (!client.Active)
                        {
                            client.Active = true;
                            client.StartDayStopService = null;
                            client.LastDayStopService = null;
                            SaveClient(client, regist: null);
                        }
                    }

                } catch(Exception e)
                {
                    //cant handle date
                }
            });

            _clientsManagerService.ClientsList.ForEach(client => {
                try
                {
                    if (client.StartDayStopService?.AddDays(1).Date >= DateTime.Today.Date)
                    {
                        if (!client.Active)
                        {
                            client.Active = false;
                            client.StartDayStopService = null;
                            client.LastDayStopService = null;
                            SaveClient(client, regist: null);
                        }
                    }

                }
                catch (Exception e)
                {
                    //cant handle date
                }
            });
        }

        public async Task SaveAllDocs(EventHandler uploadPercentageEventUpdate = null)
        {
            byte[] fileData = _fileService.GetFile(DataBaseName);
            await _firebaseService.SaveFile(DataBaseName, fileData, uploadPercentageEventUpdate);
        }

        #region Inserts
        public void InsertGlobalOrderRegist(GlobalOrderRegist globalOrderRegist)
        {
            var regist = GetGlobalOrderRegists().Find(item => item.OrderRegistDate.Date == globalOrderRegist.OrderRegistDate.Date);

            if(regist == null)
                Database.Insert(globalOrderRegist);
        }

        public GlobalOrderRegist GetGlobalOrderRegist(DateTime date)
        {
            return GetGlobalOrderRegists().Find(item => item.OrderRegistDate.Date == date);
        }
        

        public void InsertNewProduct(Product product)
        {
            Database.InsertAll(product.ReSaleValues);
            Database.InsertWithChildren(product);
        }

        public void InsertClient(Client client)
        {
            CheckDataBaseCreated(false);
            Database.Insert(client.Address);
            Database.InsertAll(client.DetailsList);
            Database.Insert(client.DailyOrders);
            Database.InsertWithChildren(client);
        }

        public void InsertClient(Client newClient, Regist regist)
        {
            Database.Insert(newClient.Address);
            Database.InsertAll(newClient.DetailsList);
            Database.InsertWithChildren(newClient);

            newClient.SetNewRegist(regist);
            Database.Insert(regist);
            Database.UpdateWithChildren(newClient);
            Database.Update(newClient);
        }

        public void InsertNewReSale(ReSaleValues reSaleValues)
        {
            Database.Insert(reSaleValues);
        }

        public void InsertNotification(Notification notification)
        {
            if(!_notificationsManagerService.HasNotificationSameDaySameClient(notification))
            {
                Database.CreateTable<Notification>();
                Database.Insert(notification);
            }
        }

        public void InsertAllDataFromXls(List<Client> clients, List<Product> products)
        {
            CheckDataBaseCreated(true);
            foreach (Client cliente in clients)
            {
                Database.Insert(cliente.Address);
                Database.InsertAll(cliente.DetailsList);

                //DailyOrder
                Database.InsertAll(cliente.SegDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.SegDailyOrder);
                Database.InsertAll(cliente.TerDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.TerDailyOrder);
                Database.InsertAll(cliente.QuaDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.QuaDailyOrder);
                Database.InsertAll(cliente.QuiDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.QuiDailyOrder);
                Database.InsertAll(cliente.SexDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.SexDailyOrder);
                Database.InsertAll(cliente.SabDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.SabDailyOrder);
                Database.InsertAll(cliente.DomDailyOrder.AllItems);
                Database.InsertWithChildren(cliente.DomDailyOrder);

                //Extra orders
                foreach (ExtraOrder extraOrder in cliente.ExtraOrdersList)
                {
                    Database.InsertAll(extraOrder.AllItems);
                    Database.InsertWithChildren(extraOrder);
                }

                Database.InsertWithChildren(cliente);
            }
            InsertAllProducts(products);
        }

        public void InsertAllProducts(List<Product> products)
        {
            foreach (Product product in products)
            {
                Database.InsertAll(product.ReSaleValues);
                Database.InsertWithChildren(product);
            }
        }

        #endregion

        #region Updates
        public void UpdateTotalOrderRegist(GlobalOrderRegist totalOrder)
        {
            Database.Update(totalOrder);
        }
        public void SaveProduct(Product productSelected)
        {
            productSelected.LastChangeDate = DateTime.Today.Date;
            Database.Update(productSelected);
            foreach(ReSaleValues reSaleValues in productSelected.ReSaleValues) 
            {
                Database.Update(reSaleValues);
            }
        }

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

        public void SaveLastPricesChangeDate(PriceChangeDate lastPricesDateChange)
        {
            if (GetPriceChangeDate().Count == 0)
                Database.Insert(lastPricesDateChange);
            else
                Database.Update(lastPricesDateChange);
        }

        public void SaveClient(Client client, Regist regist)
        {
            client.LastChangeDate = DateTime.Now;
            CheckDailyOrders(client);

            if(regist != null)
            {
                client.SetNewRegist(regist);
                Database.Insert(regist);
                Database.UpdateWithChildren(client);
            }

            Database.Update(client.Address);
            Database.Update(client);
        }

        private void CheckDailyOrders(Client client)
        {
            foreach (DailyOrder dailyOrder in client.DailyOrders)
            {
                if (dailyOrder.Id == 0)
                {
                    Database.Insert(dailyOrder);
                    Database.UpdateWithChildren(client);
                    Database.InsertAll(dailyOrder.AllItems);
                    Database.UpdateWithChildren(dailyOrder);
                }
                else
                {
                    List<DailyOrderDetails> itemsToRemove = new List<DailyOrderDetails>();
                    foreach(DailyOrderDetails dailyOrderDetails in dailyOrder.AllItems)
                    {
                        if(dailyOrderDetails.Id == 0 && dailyOrderDetails.Ammount != 0)
                        {
                            Database.Insert(dailyOrderDetails);
                            Database.UpdateWithChildren(dailyOrder);
                        }
                        else
                        {
                            if (dailyOrderDetails.Ammount == 0)
                            {
                                itemsToRemove.Add(dailyOrderDetails);
                                Database.Delete(dailyOrderDetails);
                            }
                            else
                            {
                                Database.Update(dailyOrderDetails);
                            }
                        }
                    }
                    if(dailyOrder.AllItems.Count == 0)
                    {
                        var daily = Database.GetWithChildren<DailyOrder>(dailyOrder.Id, true);
                        daily.AllItems.ForEach(item => Database.Delete(item));
                    }
                    else
                    {
                        dailyOrder.AllItems.RemoveAll(item => itemsToRemove.Contains(item));
                    }
                }
            }
        }

        public void SaveClient(Client client, ExtraOrder regist2)
        {
            client.LastChangeDate = DateTime.Now;
            Database.Insert(regist2);
            Database.UpdateWithChildren(client);
            Database.InsertAll(regist2.AllItems);
            Database.UpdateWithChildren(regist2);
        }

        public void UpdateOrder(ExtraOrder regist)
        {
            Database.Update(regist);
        }

        public void UpdateClientFromBluetooth(Client client)
        {
            try
            {
                Client oldClient = _clientsManagerService.ClientsList.Find(item => item.Id == client.Id);

                Database.Update(client);
                Database.Update(client.Address);

                UpdateRegists(client, oldClient);
                UpdateNewOrders(client, oldClient);
                //falta o update das quantidades diarias

                int pos = _clientsManagerService.ClientsList.IndexOf(oldClient);
                _clientsManagerService.ClientsList[pos] = Database.GetWithChildren<Client>(client.Id, true);
            }
            catch(Exception ex)
            {
                _dialogService.ShowErrorDialog("UpdateClientFromBluetooth", ex.Message);
                throw ex;
            }
        }

        private void UpdateNewOrders(Client client, Client oldClient)
        {
            foreach (ExtraOrder extraOrder in client.ExtraOrdersList)
            {
                var oldItem = oldClient.ExtraOrdersList.Find(item => item.Id == extraOrder.Id);
                if (oldItem == default(ExtraOrder))
                {
                    extraOrder.Id = 0;

                    Database.Insert(extraOrder);
                    Database.UpdateWithChildren(client);
                    extraOrder.AllItems.ForEach(item => {
                        item.ExtraOrderId = 0;
                        item.Id = 0;
                    });
                    Database.InsertAll(extraOrder.AllItems);
                    Database.UpdateWithChildren(extraOrder);
                }
                else
                {
                    //ignore
                    //item already exist
                }
            }
        }

        private void UpdateRegists(Client client, Client oldClient)
        {
            foreach (Regist regist in client.DetailsList)
            {
                var oldItem = oldClient.DetailsList.Find(item => item.Id == regist.Id);
                if (oldItem == default(Regist))
                {
                    regist.Id = 0;

                    Database.Insert(regist);
                }
                else
                {
                    //ignore
                    //item already exist
                }
            }
        }

        #endregion

        #region Gets
        public List<GlobalOrderRegist> GetGlobalOrderRegists()
        {
            lock (locker)
            {
                return Database.GetAllWithChildren<GlobalOrderRegist>();
            }
        }
        public List<Client> GetClients()
        {
            lock (locker)
            {
                return Database.GetAllWithChildren<Client>(recursive: true);
                // return (from c in database.Table<Client>() select c).ToList();
            }
        }

        private List<Delivery> GetDelivery()
        {
            lock (locker)
            {
                return Database.GetAllWithChildren<Delivery>(recursive: true);
            }
        }

        public List<Product> GetProducts()
        {
            lock (locker)
            {
                return Database.GetAllWithChildren<Product>();
                // return (from c in database.Table<Client>() select c).ToList();
            }
        }

        public List<PriceChangeDate> GetPriceChangeDate()
        {
            lock (locker)
            {
                return Database.GetAllWithChildren<PriceChangeDate>();
                // return (from c in database.Table<Client>() select c).ToList();
            }
        }

        public List<Notification> GetNotifications()
        {
            try
            {
                return Database.GetAllWithChildren<Notification>();
            } catch(Exception e)
            {
                return new List<Notification>();
            } 
        }

        #endregion

        #region Delete
        public void RemoveClient(int id)
        {
            Database.Delete<Client>(id);
        }

        public void RemoveClient(Client client)
        {
            Database.Delete<Client>(client.Id);
        }

        public void RemoveExtraOrder(Client client, ExtraOrder obj, Regist regist)
        {
            obj.AllItems.ForEach(item => Database.Delete<DailyOrderDetails>(item.Id));
            Database.Delete<ExtraOrder>(obj.Id);
            Database.Insert(regist);
            Database.UpdateWithChildren(client);
        }

        public void RemoveProduct(Product prod)
        {
            Database.Delete<Product>(prod.Id);
        }

        public void RemoveResaleValue(ReSaleValues resaleValue)
        {
            Database.Delete<ReSaleValues>(resaleValue.ReSaleId);
        }


        #endregion
    }
}
