using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.GlobalOrder
{
    public class CakeClientItem
    {
        public List<(int ProductId, int Ammount)> Products { get; }
        public Client Client { get; }

        public CakeClientItem(Client client, List<(int ProductId, int Ammount)> products)
        {
            Client = client;
            Products = products;
        }

    }
}
