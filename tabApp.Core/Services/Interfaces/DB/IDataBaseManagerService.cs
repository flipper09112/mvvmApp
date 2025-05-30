﻿using SQLite;
using System;
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
        bool DBRestored { get; set; }

        List<Client> GetClients();
        List<Product> GetProducts();
        List<Notification> GetNotifications();

        void InsertClient(Client contact);
        void InsertAllProducts(List<Product> products);
        void InsertAllDataFromXls(List<Client> clientsList, List<Product> productsList);

        Task LoadDataBase(EventHandler updatePercentageDownloadEvent = null);
        void SaveClient(Client client, string toRegist);
        void UpdateClientFromBluetooth(Client client);
        void SaveClient(Client client, Regist regist);
        void SaveClient(Client clientSelected, ExtraOrder order);
        void SaveProduct(Product productSelected);
        void UpdateOrder(ExtraOrder regist);
        Task SaveAllDocs(EventHandler uploadPercentageEventUpdate = null);
        void RemoveClient(Client client);
        void RemoveClient(int id);
        void RemoveExtraOrder(Client client, ExtraOrder obj, Regist regist);
        void ReloadDB();
        void InsertNotification(Notification notification);
        void InsertNewReSale(ReSaleValues reSaleValues);
        void InsertNewProduct(Product product);
        void InsertClient(Client newClient, Regist regist);
        void InsertGlobalOrderRegist(GlobalOrderRegist globalOrderRegist);
        void UpdateTotalOrderRegist(GlobalOrderRegist totalOrder);
        void RemoveProduct(Product prod);
        void LoadProducts();
        void RemoveResaleValue(ReSaleValues resaleValue);
        void SaveLastPricesChangeDate(PriceChangeDate lastPricesDateChange);
        GlobalOrderRegist GetGlobalOrderRegist(DateTime dateTime);
    }
}