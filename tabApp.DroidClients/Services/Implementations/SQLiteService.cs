using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.DroidClients.Services.Implementations
{
    class SQLiteService : ISQLiteService
    {
        public SQLiteConnection Connection()
        {
            var dbName = "MyContacts.db3";
            var path = Application.Context.GetExternalFilesDir(null).AbsolutePath;
            path = System.IO.Path.Combine(path, dbName);
            return new SQLiteConnection(path);
        }
    }
}