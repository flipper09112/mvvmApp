using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Products;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class EditProductCostValuesViewModel : BaseViewModel
    {
        private IChooseProductService _chooseProductService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IMvxNavigationService _navigationService;

        public MvxCommand SaveChangesCommand { get; }
        public Product ProductSelected => _chooseProductService.Product;

        public List<ValueChange> ValuesForChangeList { get; private set; }

        public EditProductCostValuesViewModel(IChooseProductService chooseProductService,
                                              IDataBaseManagerService dataBaseManagerService,
                                              IMvxNavigationService navigationService)
        {
            _chooseProductService = chooseProductService;
            _dataBaseManagerService = dataBaseManagerService;
            _navigationService = navigationService;

            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);
        }

        private bool CanSaveChanges()
        {
            foreach(var item in ValuesForChangeList)
            {
                if (item.NewValue != default(double))
                    return true;
            }

            return true;
        }

        private void SaveChanges()
        {
            foreach(var item in ValuesForChangeList)
            {
                if(item.NewValue != default(double))
                {
                    int index = ValuesForChangeList.IndexOf(item);

                    switch(index)
                    {
                        case 0:
                            ProductSelected.CostProduct = item.NewValue;
                            break;
                        case 1:
                            ProductSelected.Discount = item.NewValue;
                            break;
                        case 2:
                            ProductSelected.Iva = (int)item.NewValue;
                            break;
                    }
                }
            }

            _dataBaseManagerService.SaveProduct(ProductSelected);
            _navigationService.Close(this);
        }

        public override void Appearing()
        {
            ValuesForChangeList = new List<ValueChange>();

            ValuesForChangeList.Add(new ValueChange() { 
                Name = "Custo",
                Value = ProductSelected.CostProduct,
                CanContinueRefreshCommand = SaveChangesCommand
            });

            ValuesForChangeList.Add(new ValueChange()
            {
                Name = "Desconto (%)",
                Value = ProductSelected.Discount,
                CanContinueRefreshCommand = SaveChangesCommand
            });

            ValuesForChangeList.Add(new ValueChange()
            {
                Name = "IVA",
                Value = ProductSelected.Iva,
                CanContinueRefreshCommand = SaveChangesCommand
            });
        }

        public override void DisAppearing()
        {
        }
    }

    public class ValueChange
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double NewValue { get; set; }
        public MvxCommand CanContinueRefreshCommand { get; set; }
    }
}
