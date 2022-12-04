using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.WebServices.Clients;
using tabApp.Core.Services.Implementations.WebServices.Sells;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
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
        public static string APIKEY = "OZwXYysrWB2QXpA5gRVIp6zj6OT3EASKGOHozjSadVbmR79Qzrke7kpoU6f1KpK0VeU5ygg2c5mTQAp0rL3MT7v9DPeYCuohyzBEZcXLeRq7qYfJLYFmhBwtyIP7mpX3";
        //public static string APIKEY = "vvrwuk2AP4mAjNnliYav0n9lNvkZ5AbUVOdGFeQsDijeFtVkw3asV9W7Kr2Cg0V5s8M65xJrl3y9g9aTMLcrXJ0qMMyiI8MINtEx0cESne6zC0YylSpL9ln3J6M9rNwv";

        private readonly IGetVendasListaRequest _getVendasListaRequest;
        private readonly IDialogService _dialogService;
        private readonly IGetVehiclesRequest _getVehiclesRequest;
        private readonly IGetClientRequest _getClientRequest;
        private readonly ICreateSellDocumentRequest _createSellDocumentRequest;
        private readonly IAddFatProductRequest _addFatProductRequest;
        private readonly IDownloadFatRequest _downloadFatRequest;

        public TrasnportationDoc DocumentSelected { get; set; }

        public FaturationService(IGetVendasListaRequest getVendasListaRequest,
                                 IDialogService dialogService,
                                 IGetVehiclesRequest getVehiclesRequest,
                                 IGetClientRequest getClientRequest,
                                 ICreateSellDocumentRequest createSellDocumentRequest,
                                 IAddFatProductRequest addFatProductRequest,
                                 IDownloadFatRequest downloadFatRequest)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _getVehiclesRequest = getVehiclesRequest;
            _getClientRequest = getClientRequest;
            _createSellDocumentRequest = createSellDocumentRequest;
            _addFatProductRequest = addFatProductRequest;
            _downloadFatRequest = downloadFatRequest;
        }

        private TrasnportationsDocs _trasnportationsDocs;

        public TrasnportationsDocs TrasnportationsDocs
        {
            get {
                if (_trasnportationsDocs == null)
                    _trasnportationsDocs = new TrasnportationsDocs(_getVendasListaRequest, _dialogService, _createSellDocumentRequest, _downloadFatRequest);
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

        private Clients _clients;

        public Clients Clients
        {
            get
            {
                if (_clients == null)
                    _clients = new Clients(_dialogService, _getClientRequest);
                return _clients;
            }
        }

        private FatProducts _products;

        public FatProducts Products
        {
            get
            {
                if (_products == null)
                    _products = new FatProducts(_dialogService, _addFatProductRequest);
                return _products;
            }
        }
    }

    public class FatProducts
    {
        public IDialogService _dialogService { get; }

        private IAddFatProductRequest _addFatProductRequest { get; }

        public FatProducts(IDialogService dialogService, IAddFatProductRequest addFatProductRequest)
        {
            _dialogService = dialogService;
            _addFatProductRequest = addFatProductRequest;
        }

        internal async Task AddProduct(Product product)
        {
            var price = new FatPrices()
            {
                /*#if DEBUG
                                        Id = "61132",
                #endif
                #if RELEASE*/
                Id = "61432",
                //#endif
                Price = product.PVP,
                Discount = 0
            };

            var response = await _addFatProductRequest.SendAsync(new AddFatProductInput()
            {
                Reference = product.Id.ToString(),
                Description = product.Name,
                Unit = product.Unity ? "uni" : "kg",
                Vat = product.Iva,
                Type = FatProductTypeEnum.Mercadorias,
                Prices = new List<string>()
                {
                    JsonConvert.SerializeObject(price)
                }
            });

            if (!response.Success)
            {
                Debug.WriteLine(response.Error);
                return;
            }
        }
    }

    public class Clients
    {
        private IDialogService _dialogService;
        private IGetClientRequest _getClientRequest;

        public Clients(IDialogService dialogService, IGetClientRequest getClientRequest)
        {
            _dialogService = dialogService;
            _getClientRequest = getClientRequest;
        }

        public async Task<List<FatClient>> GetClient(int id)
        {
            List<FatClient> clientsList = new List<FatClient>();

            var response = await _getClientRequest.SendAsync(new GetClientInput()
            {
                Value = id.ToString(),
                SearchIn = ClientFieldEnum.Code
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return clientsList;
            }

            response.data.ForEach(client => clientsList.Add(new FatClient()
            {
                Id = client.id,
                Name = client.name,
                NIF = client.vat_number,
                Address = client.address,
                Country = client.country,
                PostalCode = client.postal_code,
                City = client.city
            }));

            return clientsList;
        }
    }

    public class Administration
    {
        private IDialogService _dialogService;
        private IGetVehiclesRequest _getVehiclesRequest;

        public Administration(IDialogService dialogService, IGetVehiclesRequest getVehiclesRequest)
        {
            _dialogService = dialogService;
            _getVehiclesRequest = getVehiclesRequest;
        }

        public async Task<List<Car>> GetVehicles()
        {
            List<Car> cars = new List<Car>(); 
            var response = await _getVehiclesRequest.SendAsync(new GetVehiclesInput());

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return cars;
            }

            response.data.ForEach(car => cars.Add(new Car()
            {
                Plate = car.license_plate,
                Id = car.id,
                Name = car.name
            }));

            return cars;
        }
    }

    public class TrasnportationsDocs
    {
        private IGetVendasListaRequest _getVendasListaRequest { get; }
        private IDialogService _dialogService;
        private ICreateSellDocumentRequest _createSellDocumentRequest;
        private IDownloadFatRequest _downloadFatRequest;

        public TrasnportationsDocs(IGetVendasListaRequest getVendasListaRequest, 
                                   IDialogService dialogService,
                                   ICreateSellDocumentRequest createSellDocumentRequest,
                                   IDownloadFatRequest downloadFatRequest)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _createSellDocumentRequest = createSellDocumentRequest;
            _downloadFatRequest = downloadFatRequest;
        }


        public async Task<List<TrasnportationDoc>> GetVendasLista()
        {
            List<TrasnportationDoc> trasnportationDocs = new List<TrasnportationDoc>();
            var response = await _getVendasListaRequest.SendAsync(new GetVendasListaInput()
            {
                Type = SellsTypes.Guias
            });

            if(!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return trasnportationDocs;
            }

            response.Data.ForEach(doc => trasnportationDocs.Add(new TrasnportationDoc()
            {
                ID = doc.id,
                Name = doc.documenttype.saft_initials + "-" + doc.serie.description + "-" + doc.documenttypeserie.document_number,
                DocumentUrl = doc.url_file,
                EmissionDate = DateTime.Parse(doc.file_last_generated)
            }));

            return trasnportationDocs;
        }

        public async void CreateDocument(FatClient clientSelected, Car vehicleSelected, DateTime dateSelected, List<FatItem> productsList)
        {
            var response = await _createSellDocumentRequest.SendAsync(new CreateSellDocumentInput()
            {
                IssueDate = DateTime.Now,
                DocumentType = DocumentTypeEnum.GuiadeTransporte,
                CustomerId = clientSelected.Id.ToString(),
                VatNumber = clientSelected.NIF,
                Address = clientSelected.Address,
                City = clientSelected.City,
                PostalCode = clientSelected.PostalCode,
                Country = clientSelected.Country,
                VatType = VatTypeEnum.Nãofazernada,
                Vehicle = vehicleSelected.Id.ToString(),
                WaybillShippingDate = dateSelected,
                //LocationOrigin = "Rua da Ciranda nº54, Santa Comba, Ponte de Lima, Portugal, 4990-740 Pte. de Lima",
                CargoLocation = "Ponte de Lima",
                Items = productsList,
                Status = StatusEnum.Terminado
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return;
            }
        }

        internal async Task<string> GetDocumentPos(TrasnportationDoc documentSelected)
        {
            var response = await _downloadFatRequest.SendAsync(new DownloadFatInput()
            {
                Id = documentSelected.ID.ToString(),
                Language = "PT",
                Format = DocFormatEnum.POS,
                PaperSize = "80",
                PaperLeftMargin = "0",
                PaperRightMargin = "0",
                PaperTopMargin = "10",
                PaperBottomMargin = "10",
                Issue = IssueTypeEnum.SecondTime,
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return null;
            }

            return response.url_file;
        }
    }
}
