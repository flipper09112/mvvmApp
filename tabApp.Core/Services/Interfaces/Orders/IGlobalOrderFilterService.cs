using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Orders
{
    public interface IGlobalOrderFilterService
    {
        bool IsActive { get; set; }
        List<ProductAmmount> ProductsList { get; set; }
        List<ProductAmmount> ProductsListCompleted { get; set; }
    }
}
