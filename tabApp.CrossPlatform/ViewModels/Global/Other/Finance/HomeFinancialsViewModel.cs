using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels.Global.Other.Finance;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class HomeFinancialsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navService;
        private readonly IAddProductToOrderService _addProductToOrderService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IDialogService _dialogService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IGlobalOrdersPastManagerService _globalOrdersPastManagerService;
        private readonly IDataBaseManagerService _databaseManagerService;

        public List<ProductAmmount> ProductsList { get; set; }
        
        public MvxCommand SelectDateCommand { get; set; }
        public MvxCommand ShowOutValueDescCommand { get; set; }
        public MvxCommand AddProductCommand { get; set; }
        public MvxCommand StatsPageCommand { get; set; }
        public MvxCommand WeekSumPageCommnand { get; set; }
        public MvxCommand SaveChangesListCommand { get; set; }
        
        public bool EditableList { get; set; }

        public HomeFinancialsViewModel(IOrdersManagerService ordersManagerService,
                                       IDialogService dialogService,
                                       IClientsManagerService clientsManagerService,
                                       IProductsManagerService productsManagerService,
                                       IGlobalOrdersPastManagerService globalOrdersPastManagerService,
                                       IDataBaseManagerService databaseManagerService,
                                       IMvxNavigationService navService,
                                       IAddProductToOrderService addProductToOrderService)
        {
            _ordersManagerService = ordersManagerService;
            _dialogService = dialogService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
            _globalOrdersPastManagerService = globalOrdersPastManagerService;
            _databaseManagerService = databaseManagerService;
            _navService = navService;
            _addProductToOrderService = addProductToOrderService;

            SelectDateCommand = new MvxCommand(SelectDate);
            SaveChangesListCommand = new MvxCommand(SaveChangesList);
            ShowOutValueDescCommand = new MvxCommand(ShowOutValueDesc);
            AddProductCommand = new MvxCommand(AddProductAsync);
            StatsPageCommand = new MvxCommand(StatsPage);
            WeekSumPageCommnand = new MvxCommand(WeekSumPage);

            EditableList = false;
        }

        private async void WeekSumPage()
        {
            await _navService.Navigate<WeekFinancialsViewModel>();
        }

        private async void StatsPage()
        {
            await _navService.Navigate<StatsViewModel>();
        }

        private async void AddProductAsync()
        {
            await _navService.Navigate<ChooseProductViewModel>();
        }

        private void ShowOutValueDesc()
        {
            _dialogService.Show("Descrição da despesa", GetOutValueDescString());
        }

        private void SaveChangesList()
        {
            var totalOrder = _globalOrdersPastManagerService.UpdateTotalOrder(ProductsList, DateSelected);
            _databaseManagerService.UpdateTotalOrderRegist(totalOrder);
        }

        public DateTime _dateSelected;
        public DateTime DateSelected
        {
            get
            {
                if (_dateSelected == DateTime.MinValue)
                {
                    _dateSelected = _globalOrdersPastManagerService.HasRegistThisDay(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1);
                }

                return _dateSelected;
            }
            set
            {
                _dateSelected = value;

                var list = _globalOrdersPastManagerService.GetOrderFromDay(DateSelected);
                list.Sort((a, b) => Math.Sign(b.Ammount - a.Ammount));

                ProductsList = list;
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public string SaldoValue
        {
            get
            {
                double count = GetInValue() - GetOutValue(); 
                return count.ToString("C");
            }
        }

        public string OutValue
        {
            get
            {
                double count = GetOutValue();
                return count.ToString("C");
            }
        }

        public string InValue 
        { 
            get
            {
                double count = GetInValue();
                return count.ToString("C");
            }
        }


        private double GetInValue()
        {
            double count = 0;

            foreach (Client client in _clientsManagerService.ClientsList)
            {
                if (!client.Active) continue;

                ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, DateSelected);

                if (order == null)
                {
                    count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(DateSelected.DayOfWeek, client));
                }
                else
                {
                    if (order.IsTotal)
                    {
                        count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                    }
                    else
                    {
                        count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(DateSelected.DayOfWeek, client));
                        count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                    }
                }
            }

            return count;
        }

        private double GetOutValue()
        {
            double count = 0;

            foreach(var item in ProductsList)
            {
                var prod = _productsManagerService.GetProductById(item.Product.Id);
                if (!item.Product.HasCostInfo())
                {
                    if(prod.HasCostInfo())
                    {
                        item.Product = prod;
                        SaveChangesList();
                    }
                    else
                    {
                        _dialogService.Show("Erro no calculo", "Existem produtos sem preço de custo associado ( " + item.Product.Name + " )");
                       // return 0;
                    }
                }
                else if(item.Product.CostProduct != prod.CostProduct || item.Product.Discount != prod.Discount)
                {
                    item.Product = prod;
                    SaveChangesList();
                }

                count += item.Product.GetCostValueWithIva() * item.Ammount;
            }

            return count;
        }

        private string GetOutValueDescString()
        {
            string desc = "";

            double count = 0;

            foreach (var item in ProductsList)
            {
                if (!item.Product.HasCostInfo())
                {
                    return "Algum produto sem valor de custo associado";
                }

                desc += item.Product.Name + " - " + (item.Product.GetCostValueWithIva() * item.Ammount).ToString("C") + "\n" + "(cost : " + item.Product.GetCostValueWithoutIva().ToString("N3") + " )\n";

                count += item.Product.GetCostValueWithIva() * item.Ammount;
            }

            desc += "Total - " + count.ToString("N2") + "\n";

            return desc;
        }

        private void SelectDate()
        {
            _dialogService.ShowDatePickerDialog(
                SelectDateAction, 
                false,
                _globalOrdersPastManagerService.MinDateRegist(),
                _globalOrdersPastManagerService.HasRegistThisDay(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1));
        }

        private void SelectDateAction(DateTime obj)
        {
            DateSelected = obj;
        }

        public override void Appearing()
        {
            bool updateDatabase = false;

            var list = _globalOrdersPastManagerService.GetOrderFromDay(DateSelected);

            if (_addProductToOrderService.ProductsSelected?.Count > 0)
            {
                _addProductToOrderService.ProductsSelected.ForEach(item => list.Add(new ProductAmmount()
                {
                    Product = item,
                    Ammount = 0
                }));

                _addProductToOrderService.Clear();
                updateDatabase = true;
            }

            list.Sort((a, b) => Math.Sign(b.Ammount - a.Ammount));
            ProductsList = list;

            if (updateDatabase)
                SaveChangesList();
        }

        public override void DisAppearing()
        {
        }
    }
}
