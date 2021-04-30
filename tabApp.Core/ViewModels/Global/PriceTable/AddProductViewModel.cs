using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class AddProductViewModel : BaseViewModel
    {
        private IProductsManagerService _productsManagerService;
        private IDataBaseManagerService _databaseManagerService;

        public List<ProductTypeEnum> ProductsTypesList => _productsManagerService.GetAllProductsTypes();

        public EventHandler GoBack;
        public MvxCommand AddProductCommand;
        public AddProductViewModel(IProductsManagerService productsManagerService,
                                   IDataBaseManagerService databaseManagerService)
        {
            _productsManagerService = productsManagerService;
            _databaseManagerService = databaseManagerService;

            AddProductCommand = new MvxCommand(AddProduct, CanAddProduct);
        }

        private void AddProduct()
        {
            List<ReSaleValues> reSaleValues = new List<ReSaleValues>();

            foreach(var item in _productsManagerService.ProductsList[0].ReSaleValues)
            {
                reSaleValues.Add(new ReSaleValues()
                { 
                    ClientId = item.ClientId,
                    Value = PriceStores
                });
            }

            Product product = new Product()
            {
                Name = ProductName,
                Id = _productsManagerService.GetUniqueId(),
                ImageReference = Reference,
                Unity = (bool)Unity,
                ProductType = ProductTypeSelected,
                PVP = Price,
                ReSaleValues = reSaleValues
            };

            _productsManagerService.ProductsList.Add(product);
            _databaseManagerService.InsertNewProduct(product);
            GoBack?.Invoke(null, null);
        }

        private bool CanAddProduct()
        {
            if (_productName == null || _productName.Equals("")) return false;
            if (_price == null || _price == 0) return false;
            if (_priceStores == null || _priceStores == 0) return false;
            if (_reference == null || _reference.Equals("")) return false;
            if (_unity == null) return false;

            return true;
        }

        public double _price;
        public double Price {
            get {
                return _price;
            }
            set {
                _price = value;
                AddProductCommand.RaiseCanExecuteChanged();
            } 
        }

        public double _priceStores;
        public double PriceStores
        {
            get
            {
                return _priceStores;
            }
            set
            {
                _priceStores = value;
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public bool? _unity;
        public bool? Unity
        {
            get
            {
                return _unity;
            }
            set
            {
                _unity = value;
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string _reference;
        public string Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string _productName;
        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public ProductTypeEnum _productTypeSelected;
        public ProductTypeEnum ProductTypeSelected
        {
            get
            {
                return _productTypeSelected;
            }
            set
            {
                _productTypeSelected = value;
                RaisePropertyChanged(nameof(ProductTypeSelected));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
