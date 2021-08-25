using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global.Other.Finance
{
    public class WeekFinancialsViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private IGlobalOrdersPastManagerService _globalOrdersPastManagerService;
        private IProductsManagerService _productsManagerService;

        public MvxCommand<DateClickType> DateClickCommand { get; set; }
        public DateClickType DateClickTypeSelected { get; private set; }
        public DateTime FirstDateSelected { get; private set; }
        public DateTime LastDateSelected { get; private set; }
        public List<DayAmmount> DaysAmmountList { get; set; }

        public WeekFinancialsViewModel(IDialogService dialogService,
                                       IGlobalOrdersPastManagerService globalOrdersPastManagerService,
                                       IProductsManagerService productsManagerService)
        {
            _dialogService = dialogService;
            _globalOrdersPastManagerService = globalOrdersPastManagerService;
            _productsManagerService = productsManagerService;

            DateClickCommand = new MvxCommand<DateClickType>(DateClick);

            FirstDateSelected = _globalOrdersPastManagerService.HasRegistThisDay(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1);
            LastDateSelected = _globalOrdersPastManagerService.HasRegistThisDay(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1);
        }

        private void DateClick(DateClickType date)
        {
            DateClickTypeSelected = date;

            _dialogService.ShowDatePickerDialog(
                SelectDate,
                true,
                date == DateClickType.FirstDate ? _globalOrdersPastManagerService.MinDateRegist() : FirstDateSelected,
                _globalOrdersPastManagerService.HasRegistThisDay(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1));
        }

        public string GetTotal()
        {
            double count = 0;

            for (DateTime counter = FirstDateSelected; counter <= LastDateSelected; counter = counter.AddDays(1))
            {
                count += GetOutValue(_globalOrdersPastManagerService.GetOrderFromDay(counter));
            }

            return count.ToString("C");
        }

        private double GetOutValue(List<ProductAmmount> ProductsList)
        {
            double count = 0;

            foreach (var item in ProductsList ?? new List<ProductAmmount>())
            {
                if (!item.Product.HasCostInfo())
                {
                    _dialogService.Show("Erro no calculo", "Existem produtos sem preço de custo associado ( " + item.Product.Name + " )");
                    return 0;
                }

                count += item.Product.GetCostValueWithIva() * item.Ammount;
            }

            return count;
        }

        private void SelectDate(DateTime date)
        {
            if (DateClickTypeSelected == DateClickType.FirstDate)
                FirstDateSelected = date;
            else if(DateClickTypeSelected == DateClickType.LastDate)
                LastDateSelected = date;

            RaisePropertyChanged(nameof(FirstDateSelected));

            GetDaysAmmountList();
        }

        public override void Appearing()
        {
            GetDaysAmmountList();
        }

        private void GetDaysAmmountList()
        {
            var list = new List<DayAmmount>();
            for (DateTime date = FirstDateSelected; date <= LastDateSelected; date = date.AddDays(1))
            {
                list.Add(new DayAmmount()
                {
                    Name = date.ToString("dddd"),
                    Value = GetOutValue(_globalOrdersPastManagerService.GetOrderFromDay(date))
                });
            }
            DaysAmmountList = list;
            RaisePropertyChanged(nameof(DaysAmmountList));
        }

        public override void DisAppearing()
        {
        }


    }

    public class DayAmmount
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public enum DateClickType
    {
        LastDate,
        FirstDate
    }
}
