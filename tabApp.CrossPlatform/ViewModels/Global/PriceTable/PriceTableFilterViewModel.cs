using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class PriceTableFilterViewModel : BaseViewModel
    {
        private IProductsManagerService _productsManagerService;
        private IPriceTableFilterService _priceTableFilterService;

        public List<PriceTableFilterItemModel> PriceTableFilterItem { get; set; }


        public EventHandler GoBack;
        public MvxCommand SaveCommand;
        public MvxCommand<Client> SelectClientCommand;
        public MvxCommand CleanFilterCommand;
        
        public PriceTableFilterViewModel(IProductsManagerService productsManagerService,
                                         IPriceTableFilterService priceTableFilterService)
        {
            _productsManagerService = productsManagerService;
            _priceTableFilterService = priceTableFilterService;

            SaveCommand = new MvxCommand(Save);
            SelectClientCommand = new MvxCommand<Client>(SelectClient);
            CleanFilterCommand = new MvxCommand(CleanFilter);

            CreateFilterItemsList();
        }

        private void CleanFilter()
        {
            _priceTableFilterService.HasFilter = false;
            _priceTableFilterService.ClientSelected = null;
            GoBack?.Invoke(null, null);
        }

        private void CreateFilterItemsList()
        {
            PriceTableFilterItem = new List<PriceTableFilterItemModel>();
            PriceTableFilterItem.Add(new PriceTableFilterItemModel()
            {
                PriceTableFilterItemType = PriceTableFilterItemEnum.Client,
                ClientsList = _productsManagerService.ClientsWithTables,
                Command = SelectClientCommand
            });
        }

        private void Save()
        {
            _priceTableFilterService.HasFilter = true;
            _priceTableFilterService.ClientSelected = ClientSelected;
            GoBack?.Invoke(null, null);
        }

        private void SelectClient(Client client)
        {
            ClientSelected = client;
        }

        public Client _clientSelected;
        public Client ClientSelected 
        { 
            get
            {
                if (_clientSelected == null && _priceTableFilterService.HasFilter)
                    return _priceTableFilterService.ClientSelected;
                return _clientSelected;
            }
            set
            {
                _clientSelected = value;
                RaisePropertyChanged(nameof(ClientSelected));
            }
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }

    public class PriceTableFilterItemModel
    {
        public PriceTableFilterItemEnum PriceTableFilterItemType { get; set; }
        public List<Client> ClientsList { get; set; }
        public MvxCommand<Client> Command { get; set; }
    }

    public enum PriceTableFilterItemEnum
    {
        Client,
        None
    }
}
