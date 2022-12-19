using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Faturation;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.ViewModels.Bases.Generic;
using Xamarin.Essentials;
using static SQLite.SQLite3;

namespace tabApp.Core.ViewModels.Global.Faturation
{
    public class TransportationDocumentsViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IFaturationService _faturationService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IDialogService _dialogService;
        private IAddProductToOrderService _addProductToOrderService;
        private IProductsManagerService _productsManagerService;

        public MvxCommand<TrasnportationDoc> OpenDocCommand { get; private set; }
        public MvxCommand CreateTransportationDocumentCommand { get; private set; }
        public MvxCommand UseTodayOrderCommand { get; private set; }
        public MvxCommand UseLastProductsListCommand { get; private set; }
        public MvxCommand AddProductCommand { get; private set; }
        public MvxCommand UpdateValueCommand { get; private set; }

        public TransportationDocumentsViewModel(IMvxNavigationService navigationService,
                                                IFaturationService faturationService,
                                                IDataBaseManagerService dataBaseManagerService,
                                                IDialogService dialogService,
                                                IAddProductToOrderService addProductToOrderService,
                                                IProductsManagerService productsManagerService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;
            _dataBaseManagerService = dataBaseManagerService;
            _dialogService = dialogService;
            _addProductToOrderService = addProductToOrderService;
            _productsManagerService = productsManagerService;

            OpenDocCommand = new MvxCommand<TrasnportationDoc>(OpenDoc);
            CreateTransportationDocumentCommand = new MvxCommand(CreateTransportationDocument, CanCreateTransportationDocument);
            UseTodayOrderCommand = new MvxCommand(UseTodayOrder);
            UseLastProductsListCommand = new MvxCommand(UseLastProductsList);
            AddProductCommand = new MvxCommand(AddProduct);
        }

        private async void AddProduct()
        {
            await _navigationService.Navigate<ChooseProductViewModel>();
        }

        private async void UseLastProductsList()
        {
            var productsListString = await SecureStorage.GetAsync(LastTransportationGuideItemsKey);
            var dateLastDoc = await SecureStorage.GetAsync(LastTransportationGuideDateKey);

            if (productsListString == null)
            {
                _dialogService.ShowErrorDialog(string.Empty, "Sem dados da última guia de transporte");
                return;
            }
            if(dateLastDoc != null)
            {
                DateSelected = DateTime.Parse(dateLastDoc);
            }

            ProductsList = JsonConvert.DeserializeObject<List<FatItem>>(productsListString);
        }

        private void UseTodayOrder()
        {
            var list = new List<FatItem>();
            var data = _dataBaseManagerService.GetGlobalOrderRegist(DateTime.Today.AddDays(1));

            if(data == null)
            {
                _dialogService.ShowErrorDialog(string.Empty, "Não existe nenhuma encomenda enviada hoje");
                return;
            }

            var dataList = data.ItemsList.OrderByDescending(item => item.Ammount).ToList();
            dataList.ForEach(itemAmmount => list.Add(new FatItem()
            {
                Id = itemAmmount.Product.Id.ToString(),
                Details = itemAmmount.Product.Name,
                Quantity = itemAmmount.Ammount.ToString(),
                //Price = itemAmmount.Product.PVP.ToString(),
                //Discount = "0",
                Vat = itemAmmount.Product.Iva.ToString(),
            }));

            ProductsList = list;
        }

        private async void CreateTransportationDocument()
        {
            IsBusy = true;
            await SecureStorage.SetAsync(LastTransportationGuideItemsKey, JsonConvert.SerializeObject(ProductsList));
            await SecureStorage.SetAsync(LastTransportationGuideDateKey, DateSelected.ToString());
            var doc = await _faturationService.TrasnportationsDocs.CreateDocument(ClientsList[0], VehicleSelected, DateSelected ?? DateTime.Today.AddDays(1), ProductsList);

            if (doc == null)
            {
                IsBusy = false;
                return;
            }

            OpenDoc(doc);
            //await _navigationService.Close(this);
            IsBusy = false;
        }

        private bool CanCreateTransportationDocument()
        {
            return DateSelected != null && ProductsList.Count != 0 && VehicleSelected != null;
        }

        private List<TrasnportationDoc> _lastTrasnportationsDocs;
        public List<TrasnportationDoc> LastTrasnportationsDocs
        {
            get => _lastTrasnportationsDocs;
            set
            {
                _lastTrasnportationsDocs = value;
                RaisePropertyChanged(nameof(LastTrasnportationsDocs));
            }
        }

        private List<Car> _vehicles;
        public List<Car> Vehicles
        {
            get => _vehicles;
            set
            {
                _vehicles = value;
                RaisePropertyChanged(nameof(Vehicles));
            }
        }

        private List<FatClient> _clientsList;
        public List<FatClient> ClientsList
        {
            get => _clientsList;
            set
            {
                _clientsList = value;
                RaisePropertyChanged(nameof(ClientsList));
            }
        }

        private List<FatItem> _productsList = new List<FatItem>();
        public List<FatItem> ProductsList
        {
            get => _productsList;
            set
            {
                _productsList = value;
                CreateTransportationDocumentCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ProductsList));
            }
        }

        private DateTime? _dateSelected;
        public DateTime? DateSelected
        {
            get => _dateSelected;
            set
            {
                _dateSelected = value;
                CreateTransportationDocumentCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        //public FatClient ClientSelected { get; set; }
        private Car _vehicleSelected;
        public Car VehicleSelected
        {
            get => _vehicleSelected;
            set
            {
                _vehicleSelected = value;
                CreateTransportationDocumentCommand.RaiseCanExecuteChanged();
            }
        }   
        
        public string LastTransportationGuideItemsKey => "TransportationDocumentsViewModel_LastTransportationGuideItemsKey";
        public string LastTransportationGuideDateKey => "TransportationDocumentsViewModel_LastTransportationGuideDateKey";

        private async void OpenDoc(TrasnportationDoc selectedDoc)
        {
            _faturationService.DocumentSelected = selectedDoc;
            await _navigationService.Navigate<DocumentViewModel>();
        }

        public override async void Appearing()
        {
            IsBusy = true;

            GetNewProduct();

            if (LastTrasnportationsDocs != null)
            {
                IsBusy = false;
                return;
            }

            LastTrasnportationsDocs = await _faturationService.TrasnportationsDocs.GetVendasLista(SellsTypes.Guias);
            Vehicles = await _faturationService.Administration.GetVehicles();
            ClientsList = await _faturationService.Clients.GetClient("Consumidor final");

            IsBusy = false;
        }
        private void GetNewProduct()
        {
            if (_addProductToOrderService.ProductsSelected.Count > 0)
            {
                _addProductToOrderService.ProductsSelected.ForEach(product => ProductsList.Add(new FatItem()
                {
                    Id = product.Id.ToString(),
                    Details = _productsManagerService.GetProductById(product.Id).Name,
                    Discount = "0",
                    Price = product.PVP.ToString(),
                    Vat = product.Iva.ToString() ?? "NaN",
                    Quantity = "0",
                }));
            }
            _addProductToOrderService.ProductsSelected.Clear();
        }

        public override void DisAppearing()
        {
        }
    }
}
