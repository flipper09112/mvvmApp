using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global
{
    public class GlobalOrderViewModel : BaseViewModel
    {
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IProductsManagerService _productsManagerService;

        public GlobalOrderViewModel(IOrdersManagerService ordersManagerService, IProductsManagerService productsManagerService)
        {
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
        }

        private List<ProductAmmount> _productsList;
        public List<ProductAmmount> ProductsList {
            get
            {
                return _productsList; 
            }
            set
            {
                _productsList = value;
                _productsList.Sort((a, b) => Math.Sign(b.Ammount - a.Ammount));
                RaisePropertyChanged(nameof(ProductsList));
            }
        }

        private List<(Client Client, ExtraOrder ExtraOrder)> _tomorrowOrders; 
        public List<(Client Client, ExtraOrder ExtraOrder)> TomorrowOrders { 
            get
            {
                return _tomorrowOrders;
            }
            set
            {
                _tomorrowOrders = value;
            }
        }

        private List<CakeClientItem> _cakesClients;
        public List<CakeClientItem> CakesClients
        {
            get
            {
                return _cakesClients;
            }
            set
            {
                _cakesClients = value;
            }
        }

        public override void Appearing()
        {
            ProductsList = _ordersManagerService.GetTotalOrder(DateTime.Today.AddDays(1));
            TomorrowOrders = _ordersManagerService.TomorrowOrders;
            CakesClients = _ordersManagerService.CakesClientsTomorrow;
        }

        public override void DisAppearing()
        {
        }

        public string GetOrderDesc(ExtraOrder obj)
        {
            string details = "";
            foreach (var item in obj.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);
                details += product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2")) + "\n";
            }
            return details;
        }

        public string GetTextForSentToEmail()
        {
            string info = "";
            foreach(var item in ProductsList)
            {
                info += item.Product.Name + " - " + (item.Product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N3")) + "\n\n";
            }
            info += "----------------------------------------\n\n";
            return info;
        }
    }
}
