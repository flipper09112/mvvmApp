﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global
{
    public class GlobalOrderViewModel : BaseViewModel
    {
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IGlobalOrderFilterService _globalOrderFilterService;
        private readonly IDataBaseManagerService _dataBaseManagerService;
        public MvxCommand SaveAllDataCommand;
        public MvxCommand SetMoreDaysOrderCommand;

        public GlobalOrderViewModel(IOrdersManagerService ordersManagerService, 
                                    IProductsManagerService productsManagerService,
                                    IMvxNavigationService navigationService,
                                    IDataBaseManagerService dataBaseManagerService,
                                    IGlobalOrderFilterService globalOrderFilterService)
        {
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
            _navigationService = navigationService;
            _globalOrderFilterService = globalOrderFilterService;
            _dataBaseManagerService = dataBaseManagerService;

            SaveAllDataCommand = new MvxCommand(SaveAllData);
            SetMoreDaysOrderCommand = new MvxCommand(SetMoreDaysOrder);
        }

        private async void SetMoreDaysOrder()
        {
            await _navigationService.Navigate<GlobalOrderSelectDaysViewModel>();
        }

        private void SaveAllData()
        {
            AddTotalOrderRegist();
            _dataBaseManagerService.SaveAllDocs();
        }

        private void AddTotalOrderRegist()
        {
            List<ProductAmmount> list;
            if (_globalOrderFilterService.IsActive)
                list = _globalOrderFilterService.ProductsListCompleted;
            else
            {
                list = _ordersManagerService.GetTotalOrderWithoutFilter(DateTime.Today.AddDays(1));
            }

            SetValuesFromUser(list);

            var json = JsonConvert.SerializeObject(list);
             _dataBaseManagerService.InsertGlobalOrderRegist(new GlobalOrderRegist() { 
                OrderRegistDate = DateTime.Today.AddDays(1),
                JsonData = json
            });
        }

        private void SetValuesFromUser(List<ProductAmmount> list)
        {
            foreach(var item in list)
            {
                var productUpdated = ProductsList.Find(prod => prod.Product.Id == item.Product.Id);

                if (productUpdated != null) {
                    item.Ammount = productUpdated.Ammount;
                }
            }
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
            if (_globalOrderFilterService.IsActive)
                ProductsList = _globalOrderFilterService.ProductsList;
            //else if(!_globalOrderFilterService.IsActive && _globalOrderFilterService.ProductsList != null)
            else
            {
                _globalOrderFilterService.ProductsList = null;
                ProductsList = _ordersManagerService.GetTotalOrder(DateTime.Today.AddDays(1));
            }
            
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

            foreach(var item in CakesClients)
            {
                if(!item.Selected)
                {
                    foreach(var products in item.Products)
                    {
                        info += products.ProductName + " - " + products.Ammount + "\n";
                    }
                    info += "----------------------------------------\n\n";
                }
            }

            return info;
        }
    }
}
