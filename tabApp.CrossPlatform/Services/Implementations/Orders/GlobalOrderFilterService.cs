using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Orders;

namespace tabApp.Core.Services.Implementations.Orders
{
    public class GlobalOrderFilterService : IGlobalOrderFilterService
    {
        public bool IsActive { get ; set ; }

        public List<ProductAmmount> ProductsList { get; set; }
        public List<ProductAmmount> ProductsListCompleted { get; set; }
    }
}
