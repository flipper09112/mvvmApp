using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Faturation.Helpers;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Implementations.WebServices.Clients;
using tabApp.Core.Services.Implementations.WebServices.Products;
using tabApp.Core.Services.Implementations.WebServices.Sells;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Interfaces.WebServices.Admin;
using tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Clients;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace tabApp.Core.Services.Implementations.Faturation
{
    public class FaturationService : IFaturationService
    {
        public static string BaseUrl = "https://facturalusa.pt/api/v1";

//#if DEBUG
        //public static string APIKEY = "qBPFofneE5YeYv8DDVBR0d5f2mq0wIY6DMhmki5BzAFOEZ1mdEfIf9boZtnHBr3ckFrYm48ycUuGpoZESMQGlfeH91rHbceG8oILTtrSklhRcTkMBxztQtbK0QwPtpjf";
//#elif RELEASE
        public static string APIKEY = "8XmZy4zUHx0dM7jqo1JSfAPIRcefKCLSWRO730uFHrD1upEh18KIkTiXmzRalP4LxyyjL3szNVSz729hQ0aNqhL8bgQXenWiQJI8nW5WrygmJX01D3CQE5SGD5n3Q6EX";
//#endif

        private readonly IGetVendasListaRequest _getVendasListaRequest;
        private readonly IDialogService _dialogService;
        private readonly IGetVehiclesRequest _getVehiclesRequest;
        private readonly IGetClientRequest _getClientRequest;
        private readonly ICreateSellDocumentRequest _createSellDocumentRequest;
        private readonly IAddFatProductRequest _addFatProductRequest;
        private readonly IDownloadFatRequest _downloadFatRequest;
        private readonly IDuplicateWayBillRequest _duplicateWayBillRequest;
        private readonly IUpdateFatRequest _updateFatRequest;
        private readonly IGetFatProductRequest _getFatProductRequest;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IUpdateFatProductRequest _updateFatProductRequest;
        private readonly IDeleteFatProductRequest _deleteFatProductRequest;
        private readonly ICreateFatClientRequest _createFatClientRequest;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IDataBaseManagerService _dataBaseManagerService;

        public TrasnportationDoc DocumentSelected { get; set; }

        public FaturationService(IGetVendasListaRequest getVendasListaRequest,
                                 IDialogService dialogService,
                                 IGetVehiclesRequest getVehiclesRequest,
                                 IGetClientRequest getClientRequest,
                                 ICreateSellDocumentRequest createSellDocumentRequest,
                                 IAddFatProductRequest addFatProductRequest,
                                 IDownloadFatRequest downloadFatRequest,
                                 IDuplicateWayBillRequest duplicateWayBillRequest,
                                 IUpdateFatRequest updateFatRequest,
                                 IGetFatProductRequest getFatProductRequest,
                                 IProductsManagerService productsManagerService,
                                 IUpdateFatProductRequest updateFatProductRequest,
                                 IDeleteFatProductRequest deleteFatProductRequest,
                                 ICreateFatClientRequest createFatClientRequest,
                                 IClientsManagerService clientsManagerService,
                                 IDataBaseManagerService dataBaseManagerService)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _getVehiclesRequest = getVehiclesRequest;
            _getClientRequest = getClientRequest;
            _createSellDocumentRequest = createSellDocumentRequest;
            _addFatProductRequest = addFatProductRequest;
            _downloadFatRequest = downloadFatRequest;
            _duplicateWayBillRequest = duplicateWayBillRequest;
            _updateFatRequest = updateFatRequest;
            _getFatProductRequest = getFatProductRequest;
            _productsManagerService = productsManagerService;
            _updateFatProductRequest = updateFatProductRequest;
            _deleteFatProductRequest = deleteFatProductRequest;
            _createFatClientRequest = createFatClientRequest;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;
        }

        private TrasnportationsDocs _trasnportationsDocs;

        public TrasnportationsDocs TrasnportationsDocs
        {
            get {
                if (_trasnportationsDocs == null)
                    _trasnportationsDocs = new TrasnportationsDocs(_getVendasListaRequest, 
                                                                   _dialogService, 
                                                                   _createSellDocumentRequest, 
                                                                   _downloadFatRequest, 
                                                                   _duplicateWayBillRequest,
                                                                   _updateFatRequest,
                                                                   Products,
                                                                   Clients);
                return _trasnportationsDocs; 
            }
        }

        private Administration _administration;

        public Administration Administration
        {
            get
            {
                if (_administration == null)
                    _administration = new Administration(_dialogService, _getVehiclesRequest);
                return _administration;
            }
        }

        private Helpers.Clients _clients;

        public Helpers.Clients Clients
        {
            get
            {
                if (_clients == null)
                    _clients = new Helpers.Clients(_dialogService, 
                                                   _getClientRequest, 
                                                   _createFatClientRequest,
                                                   _clientsManagerService);
                return _clients;
            }
        }

        private FatProducts _products;

        public FatProducts Products
        {
            get
            {
                if (_products == null)
                    _products = new FatProducts(_dialogService, 
                                                _addFatProductRequest, 
                                                _getFatProductRequest,
                                                _productsManagerService,
                                                _updateFatProductRequest,
                                                _deleteFatProductRequest,
                                                _dataBaseManagerService);
                return _products;
            }
        }

        public Client ClientSelected { get; set; }

        public List<FatItem> GetItemsRemainingFromGuia(TrasnportationDoc guiaSelected, List<TrasnportationDoc> faturationDocs)
        {
            List<FatItem> fatItems = new List<FatItem>();
            if (guiaSelected == null) return null;
            List<Item> itemsRemaining = guiaSelected.ProductItems;

            foreach(var fat in faturationDocs)
            {
                if (fat.StartTravelDate != guiaSelected.StartTravelDate)
                    continue;

                foreach(var product in fat.ProductItems)
                {
                    var findProduct = itemsRemaining.Find(item => item.item.reference == product.item.reference);
                    if(findProduct != null)
                        findProduct.quantity -= product.quantity;
                }
            }

            itemsRemaining = itemsRemaining.Where(item => item.quantity > 0).ToList();

            itemsRemaining.ForEach(product => fatItems.Add(new FatItem()
            {
                Id = product.item.reference.ToString(),
                Details = product.item.description,
                Discount = product.discount.ToString(),
                Price = _productsManagerService.GetProductById(int.Parse(product.item.reference)).PVP.ToString(),
                Vat = product?.vat?.tax.ToString() ?? "NaN",
                Quantity = product.quantity.ToString(),
            }));

            return fatItems;
        }
    }
}
