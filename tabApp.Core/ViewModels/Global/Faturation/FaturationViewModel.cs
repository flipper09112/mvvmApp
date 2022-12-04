using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
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

        public MvxCommand<TrasnportationDoc> OpenDocCommand { get; private set; }
        public MvxCommand CreateTransportationDocumentCommand { get; private set; }
        public MvxCommand UseTodayOrderCommand { get; private set; }
        public MvxCommand UseLastProductsListCommand { get; private set; }
        public string LastInvoiveGuideItemsKey => "FaturationViewModel_LastTransportationGuideItemsKey";

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

        public FaturationViewModel(IMvxNavigationService navigationService,
                                   IFaturationService faturationService,
                                   IDataBaseManagerService dataBaseManagerService,
                                   IDialogService dialogService)
        {
            _navigationService = navigationService;
            _faturationService = faturationService;
            _dataBaseManagerService = dataBaseManagerService;
            _dialogService = dialogService;

            OpenDocCommand = new MvxCommand<TrasnportationDoc>(OpenDoc);
            CreateTransportationDocumentCommand = new MvxCommand(CreateTransportationDocument, CanCreateTransportationDocument);
            UseTodayOrderCommand = new MvxCommand(UseTodayOrder);
            UseLastProductsListCommand = new MvxCommand(UseLastProductsList);
        }

        private async void UseLastProductsList()
        {
            var productsListString = await SecureStorage.GetAsync(LastInvoiveGuideItemsKey);

            if (productsListString == null)
            {
                _dialogService.ShowErrorDialog(string.Empty, "Sem dados da última guia de transporte");
                return;
            }
            ProductsList = JsonConvert.DeserializeObject<List<FatItem>>(productsListString);
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
            }));

            ProductsList = list;
        }

        private async void OpenDoc(TrasnportationDoc selectedDoc)
        {
            _faturationService.DocumentSelected = selectedDoc;
            await _navigationService.Navigate<DocumentViewModel>();
        }

        private void CreateTransportationDocument()
        {
            SecureStorage.SetAsync(LastInvoiveGuideItemsKey, JsonConvert.SerializeObject(ProductsList));
            //_faturationService.TrasnportationsDocs.CreateDocument(ClientSelected, VehicleSelected, DateSelected ?? DateTime.Today.AddDays(1), ProductsList);
        }

        private bool CanCreateTransportationDocument()
        {
            return ProductsList.Count != 0;
        }

        public override async void Appearing()
        {
            IsBusy = true;

            if (LastTrasnportationsDocs != null)
            {
                IsBusy = false;
                return;
            }

            LastTrasnportationsDocs = await _faturationService.TrasnportationsDocs.GetVendasLista();

            IsBusy = false;
        }

        public override void DisAppearing()
        {
        }
    }
}
