using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.GlobalOrder
{
    public class CakeClientItem
    {
        public List<(string ProductName, int Ammount)> Products { get; }
        public Client Client { get; }

        public CakeClientItem(Client client, List<(string ProductName, int Ammount)> products)
        {
            Client = client;
            Products = products;
        }

    }
}
