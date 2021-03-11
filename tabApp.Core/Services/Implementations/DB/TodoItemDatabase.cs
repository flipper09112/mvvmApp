using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Enums;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.DB
{
    public class TodoItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<TodoItemDatabase> Instance = new AsyncLazy<TodoItemDatabase>(async () =>
        {
            var instance = new TodoItemDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Client>();
            return instance;
        });

        public TodoItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Client>> GetItemsAsync()
        {
            return Database.Table<Client>().ToListAsync();
        }

        public Task<List<Client>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<Client>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Client> GetItemAsync(int id)
        {
            return Database.Table<Client>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Client item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Client item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
