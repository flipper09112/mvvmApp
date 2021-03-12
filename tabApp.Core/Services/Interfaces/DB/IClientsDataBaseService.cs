using System.Collections.Generic;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.DB
{
    public interface IClientsDataBaseService
    {
        List<Client> GetContacts();

        void Insert(Client contact);

        void RemoveContact(int id);
    }
}