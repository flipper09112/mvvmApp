﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels;

namespace tabApp.Core
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IClientsListFilterService _clientsListFilterService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IProductsManagerService _productsManagerService;

        public MvxAsyncCommand<Client> ShowClientPage { get; private set; }

        public HomeViewModel(IMvxNavigationService navigationService, 
                            IClientsManagerService clientsManagerService,
                            IChooseClientService chooseClientService,
                            IClientsListFilterService clientsListFilterService,
                            IOrdersManagerService ordersManagerService,
                            IProductsManagerService productsManagerService)
        {
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _chooseClientService = chooseClientService;
            _clientsListFilterService = clientsListFilterService;
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;

            ShowClientPage = new MvxAsyncCommand<Client>(ShowClientPageAction);
        }

        private async Task ShowClientPageAction(Client clientSelected)
        {
            _chooseClientService.SelectClient(clientSelected);
            await _navigationService.Navigate<ClientPageViewModel>();
        }

        public List<Client> ClientsList
        {
            get
            {
                if(_clientsListFilterService.HasFilter)
                {
                    return _clientsListFilterService.FilterClients(_clientsManagerService.ClientsList);
                }

                return _clientsManagerService.ClientsList;
            }
        }

        private List<SecondaryOptions> _tabsOptions;
        public List<SecondaryOptions> TabsOptions
        {
            get
            {
                return _tabsOptions;
            }
            set
            {
                _tabsOptions = value;
                RaisePropertyChanged(nameof(TabsOptions));
            }
        }

        public override void Appearing()
        {
            TabsOptions = GetSecondaryOptions();
        }

        private List<SecondaryOptions> GetSecondaryOptions()
        {
            List<SecondaryOptions> items = new List<SecondaryOptions>();
            items.Add(new OrdersPage("Encomendas", _ordersManagerService.TodayOrders));
            items.Add(new SecondaryOptions("Restante"));
            return items;
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

        public override void DisAppearing()
        {
        }
    }

    public class SecondaryOptions
    {
        public string Name { get; }

        public SecondaryOptions(string name)
        {
            Name = name;
        }
    }

    public class OrdersPage : SecondaryOptions
    {
        public List<(Client Client, ExtraOrder ExtraOrder)> Value { get; }

        public OrdersPage(string name, List<(Client Client, ExtraOrder ExtraOrder)> value) : base(name)
        {
            Value = value;
        }
    }
}