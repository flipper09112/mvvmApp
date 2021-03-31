using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Products
{
    public interface IPriceTableFilterService
    {
        bool HasFilter { get; set; }
        Client ClientSelected { get; set; }
    }
}
