using Android.App;
using SQLite;
using tabApp.Core.Services.Interfaces.DB;

namespace tabApp.DroidWear.Services.Implementations
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