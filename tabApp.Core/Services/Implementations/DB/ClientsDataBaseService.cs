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

namespace tabApp.Core.Services.Implementations.DB
{
    public class ClientsDataBaseService : IClientsDataBaseService
    {
        private SQLiteConnection database;
        static object locker = new object();
        private ISQLiteService _sQLiteService;

        public ClientsDataBaseService(ISQLiteService sQLiteService)
        {
            _sQLiteService = sQLiteService;
        }

        private void CheckDataBaseCreated()
        {
            if (database != null) return;

            try
            {
                database = _sQLiteService.Connection();
                database.DropTable<Address>();
                database.DropTable<Regist>();
                database.DropTable<Client>();
                database.CreateTable<Regist>();
                database.CreateTable<Address>();
                database.CreateTable<Client>();

            } catch (NotSupportedException e) {
                Debug.WriteLine(e.Message);
            }
        }

        public List<Client> GetContacts()
        {
            CheckDataBaseCreated();
            lock (locker)
            {
                return database.GetAllWithChildren<Client>();
                // return (from c in database.Table<Client>() select c).ToList();
            }
            
        }

        public void Insert(Client contact)
        {
            CheckDataBaseCreated();
            //database.Insert(contact);
            //database.Insert(contact);
            database.Insert(contact.Address);
            database.InsertAll(contact.DetailsList);
            database.InsertWithChildren(contact);

        }

        public void RemoveContact(int id)
        {
            CheckDataBaseCreated();
            database.Delete<Client>(id);
        }
    }
}
