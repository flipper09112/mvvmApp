using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Faturation;
using tabApp.Core.Services.Implementations.Faturation.Helpers;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.ViewModels.Bases.Generic;
using Xamarin.Essentials;

namespace tabApp.Core.ViewModels.Global.Faturation
{
    public class FaturationViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IFaturationService _faturationService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IDialogService _dialogService;
        private IProductsManagerService _productsManagerService;
        private IChooseClientService _chooseClientService;
        private IClientsManagerService _clientsManagerService;
        private IAddProductToOrderService _addProductToOrderService;

        public MvxCommand<TrasnportationDoc> OpenDocCommand { get; private set; }
        public MvxCommand CreateFaturaSimplesCommand { get; private set; }
        public MvxCommand UseTodayOrderCommand { get; private set; }
        public MvxCommand UseLastProductsListCommand { get; private set; }
        public MvxCommand AddProductCommand { get; private set; }
        public MvxCommand UpdateValueCommand { get; private set; }
        public MvxCommand UseTemplateOneCommand { get; private set; }
        public MvxCommand UseTemplateTwoCommand { get; private set; }
        public MvxCommand UseTemplateThreeCommand { get; private set; }
        public MvxCommand SetTemplateOneCommand { get; private set; }
        public MvxCommand SetTemplateTwoCommand { get; private set; }
        public MvxCommand SetTemplateThreeCommand { get; private set; }

        private bool _fatForClient;
        private Client _clientSelectedApp;
        private FatClient _clientSelected;

        public string LastInvoiveGuideItemsKey => "FaturationViewModel_LastTransportationGuideItemsKey";
        public string TemplateOneKey => "TemplateOneKey";
        public string TemplateTwoKey => "TemplateTwoKey";
        public string TemplateThreeKey => "TemplateThreeKey";

        private List<FatItem> _productsList = new List<FatItem>();
        public List<FatItem> ProductsList
        {
            get => _productsList;
            set
            {
                _productsList = value;
                CreateFaturaSimplesCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ProductsList));
                RaisePropertyChanged(nameof(TotalFatValue));
            }
        }

        private List<TrasnportationDoc> _faturationDocs;
        public List<TrasnportationDoc> FaturationDocs
        {
            get => _faturationDocs;
            set
            {
                _faturationDocs = value;
                RaisePropertyChanged(nameof(FaturationDocs));
            }
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

        public List<FatClient> Client { get; private set; }

        private List<FatItem> _productsRemaining = new List<FatItem>();
        public List<FatItem> ProductsRemaining
        {
            get => _productsRemaining;
            set
            {
                _productsRemaining = value;
                RaisePropertyChanged(nameof(ProductsRemaining));
            }
        }

        private TrasnportationDoc _guiaSelected;
        public TrasnportationDoc GuiaSelected
        {
            get => _guiaSelected;
            set
            {
                _guiaSelected = value;
                IsBusy = true;
                GetItems();
                IsBusy = false;
                ProductsRemaining = _faturationService.GetItemsRemainingFromGuia(GuiaSelected, FaturationDocs);
                CreateFaturaSimplesCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(GuiaSelected));
            }
        }

        public FaturationViewModel(IMvxNavigationService navigationService,
                                   IFaturationService faturationService,
                                   IDataBaseManagerService dataBaseManagerService,
                                   IDialogService dialogService,
                                   IProductsManagerService productsManagerService,
                                   IChooseClientService chooseClientService,
                                   IClientsManagerService clientsManagerService,
                                   IAddProductToOrderService addProductToOrderService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;
            _dataBaseManagerService = dataBaseManagerService;
            _dialogService = dialogService;
            _productsManagerService = productsManagerService;
            _chooseClientService = chooseClientService;
            _clientsManagerService = clientsManagerService;
            _addProductToOrderService = addProductToOrderService;

            OpenDocCommand = new MvxCommand<TrasnportationDoc>(OpenDoc);
            CreateFaturaSimplesCommand = new MvxCommand(CreateTransportationDocument, CanCreateTransportationDocument);
            UseTodayOrderCommand = new MvxCommand(UseTodayOrder);
            UseLastProductsListCommand = new MvxCommand(UseLastProductsList);
            AddProductCommand = new MvxCommand(AddProduct);
            UpdateValueCommand = new MvxCommand(UpdateValue);
            UseTemplateOneCommand = new MvxCommand(UseTemplateOne, CanUseTemplateOne);
            UseTemplateTwoCommand = new MvxCommand(UseTemplateTwo, CanUseTemplateTwo);
            UseTemplateThreeCommand = new MvxCommand(UseTemplateThree, CanUseTemplateThree);
            SetTemplateOneCommand = new MvxCommand(SetTemplateOne);
            SetTemplateTwoCommand = new MvxCommand(SetTemplateTwo);
            SetTemplateThreeCommand = new MvxCommand(SetTemplateThree);

            _fatForClient = _faturationService.ClientSelected != null;
            _clientSelectedApp = _faturationService.ClientSelected;

            if(_clientSelectedApp != null)
                _clientSelected = new FatClient()
                {
                    Id = _faturationService.ClientSelected.Id,
                    Name = _faturationService.ClientSelected.Name,
                    Country = "Portugal",
                    Address = _faturationService.ClientSelected.Address.AddressDesc,
                    NIF = _faturationService.ClientSelected.NIF.ToString(),
                };

            _faturationService.ClientSelected = null;
        }

        private async void SetTemplateThree()
        {
            IsBusy = true;

            var data = await SecureStorage.GetAsync(TemplateThreeKey);
            if (data == null)
                await SecureStorage.SetAsync(TemplateThreeKey, JsonConvert.SerializeObject(ProductsList));
            else
                SecureStorage.Remove(TemplateThreeKey);

            await RaisePropertyChanged(nameof(TemplateThreeKey));

            IsBusy = false;
        }

        private async void SetTemplateTwo()
        {
            IsBusy = true;

            var data = await SecureStorage.GetAsync(TemplateTwoKey);
            if (data == null)
                await SecureStorage.SetAsync(TemplateTwoKey, JsonConvert.SerializeObject(ProductsList));
            else
                SecureStorage.Remove(TemplateTwoKey);

            await RaisePropertyChanged(nameof(TemplateTwoKey));

            IsBusy = false;
        }

        private async void SetTemplateOne()
        {
            IsBusy = true;

            var data = await SecureStorage.GetAsync(TemplateOneKey);
            if (data == null)
                await SecureStorage.SetAsync(TemplateOneKey, JsonConvert.SerializeObject(ProductsList));
            else
                SecureStorage.Remove(TemplateOneKey);

            await RaisePropertyChanged(nameof(TemplateOneKey));
            IsBusy = false;
        }

        private bool CanUseTemplateThree()
        {
            return SecureStorage.GetAsync(TemplateThreeKey).Result != null;
        }

        private bool CanUseTemplateTwo()
        {
            return SecureStorage.GetAsync(TemplateTwoKey).Result != null;
        }

        private bool CanUseTemplateOne()
        {
            return SecureStorage.GetAsync(TemplateOneKey).Result != null;
        }

        private void UseTemplateThree()
        {
            UseTemplate(TemplateThreeKey);
        }

        private void UseTemplateTwo()
        {
            UseTemplate(TemplateTwoKey);
        }

        private void UseTemplateOne()
        {
            UseTemplate(TemplateOneKey);
        }

        private async void UseTemplate(string key)
        {
            IsBusy = true;

            var data = await SecureStorage.GetAsync(key);

            if (data != null)
            {
                var productsList = JsonConvert.DeserializeObject<List<FatItem>>(data);
                //used for update iva values of products
                foreach (var prod in productsList)
                {
                    var product = _productsManagerService.GetProductById(int.Parse(prod.Id));

                    if (product != null)
                    {
                        prod.Vat = product.Iva.ToString();
                        prod.VatExemption = product.IsencaoIva;
                    }
                }

                ProductsList = productsList;
            }

            IsBusy = false;
        }

        private void UpdateValue()
        {
            RaisePropertyChanged(nameof(TotalFatValue));
        }

        private async void AddProduct()
        {
            await _navigationService.Navigate<ChooseProductViewModel>();
        }

        public string ClientName => _fatForClient ? (_clientSelected.Name + "["+_clientSelected.NIF+"]") : "Consumidor final";
        public bool FatForClient => _fatForClient;

        public string TotalFatValue
        {
            get
            {
                double value = 0;
                ProductsList?.ForEach(item => {
                    value += double.Parse(item.Price) * double.Parse(item.Quantity);
                });

                return value.ToString("C");
            }
        }

        public bool FaturaRecibo { get; set; } = true;

        private async void UseLastProductsList()
        {
            var productsListString = await SecureStorage.GetAsync(LastInvoiveGuideItemsKey);

            if (productsListString == null)
            {
                _dialogService.ShowErrorDialog(string.Empty, "Sem dados da última guia de transporte");
                return;
            }

            var productsList = JsonConvert.DeserializeObject<List<FatItem>>(productsListString);

            //used for update iva values of products
            foreach (var prod in productsList)
            {
                var product = _productsManagerService.GetProductById(int.Parse(prod.Id));

                if (product != null)
                {
                    prod.Vat = product.Iva.ToString();
                    prod.VatExemption = product.IsencaoIva;
                }
            }

            ProductsList = productsList;
        }

        private void UseTodayOrder()
        {
            var list = new List<FatItem>();
            var data = _dataBaseManagerService.GetGlobalOrderRegist(DateTime.Today.AddDays(1));

            if (data == null)
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
                VatExemption = itemAmmount.Product.IsencaoIva
            }));

            ProductsList = list;
        }

        private async void OpenDoc(TrasnportationDoc selectedDoc)
        {
            _faturationService.DocumentSelected = selectedDoc;
            await _navigationService.Navigate<DocumentViewModel>();
        }

        private async void CreateTransportationDocument()
        {
            IsBusy = true;
            SecureStorage.SetAsync(LastInvoiveGuideItemsKey, JsonConvert.SerializeObject(ProductsList));
            var fatId = await _faturationService.TrasnportationsDocs.CreateFatByWayBill(GuiaSelected);

            if (fatId == null)
            {
                IsBusy = false;
                return;
            }

            var doc = await _faturationService.TrasnportationsDocs.UpdateFatDocument(fatId, _clientSelected ?? Client[0], ProductsList, GuiaSelected, FaturaRecibo);

            if (doc == null)
            {
                IsBusy = false;
                return;
            }

            IsBusy = false;
            //await _navigationService.Close(this);
            OpenDoc(doc);
        }

        private bool CanCreateTransportationDocument()
        {
            return ProductsList.Count != 0 && GuiaSelected != null;
        }

        public override async void Appearing()
        {
            IsBusy = true;

            GetNewProduct();

            if (FaturationDocs != null)
            {
                IsBusy = false;
                return;
            }

            FaturationDocs = await _faturationService.TrasnportationsDocs.GetVendasLista(SellsTypes.Facturação, _clientSelectedApp?.Id);
            LastTrasnportationsDocs = await _faturationService.TrasnportationsDocs.GetVendasLista(SellsTypes.Guias);
            Client = await _faturationService.Clients.GetClient(_fatForClient ? _clientSelectedApp?.Id.ToString() : "Consumidor final");

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
                    Price = !_fatForClient ? product.PVP.ToString() : _productsManagerService.GetProductAmmount(_clientSelectedApp.Id, product).ToString(),
                    Vat = product.Iva.ToString() ?? "NaN",
                    VatExemption = product.IsencaoIva,
                    Quantity = "0",
                }));
            }
            _addProductToOrderService.ProductsSelected.Clear();
        }

        private void GetItems()
        {
            if (!_fatForClient)
                GetItemsFromGuia();
            else
                GetItemsFromClient();
        }

        private void GetItemsFromClient()
        {
            List<FatItem> list = new List<FatItem>();

            var productsListAndAmmounts = _clientsManagerService.GetTotalProductsFromClient(_clientSelectedApp, _chooseClientService.PayTo);

            productsListAndAmmounts.ForEach(product => list.Add(new FatItem()
            {
                Id = product.productId.ToString(),
                Details = _productsManagerService.GetProductById(product.productId).Name,
                Discount = "0",
                Price = _productsManagerService.GetProductAmmount(_clientSelectedApp.Id, _productsManagerService.GetProductById(product.productId)).ToString(),
                Vat = _productsManagerService.GetProductById(product.productId).Iva.ToString() ?? "NaN",
                VatExemption = _productsManagerService.GetProductById(product.productId).IsencaoIva,
                Quantity = product.ammount.ToString(),
            }));

            list = list.OrderByDescending(item => item.Quantity).ToList();
            ProductsList = list;
        }

        private void GetItemsFromGuia()
        {
            List<FatItem> list = new List<FatItem>();

            LastTrasnportationsDocs.Find(guia => guia.ID == GuiaSelected.ID).ProductItems.ForEach(product => list.Add(new FatItem()
            {
                Id = product.item.reference.ToString(),
                Details = product.item_details,
                Discount = product.discount.ToString(),
                Price = _productsManagerService.GetProductById(int.Parse(product.item.reference)).PVP.ToString(),
                Vat = product?.vat?.tax.ToString() ?? "NaN",
                VatExemption = _productsManagerService.GetProductById(int.Parse(product.item.reference)).IsencaoIva,
                Quantity = product.quantity.ToString(),
            }));

            ProductsList = list;
        }

        public override void DisAppearing()
        {
        }
    }
}
