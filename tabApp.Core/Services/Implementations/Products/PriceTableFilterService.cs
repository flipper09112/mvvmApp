using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public class PriceTableFilterService : IPriceTableFilterService
    {
        public bool HasFilter { get; set; }
        public Client ClientSelected { get; set; }
    }
}
