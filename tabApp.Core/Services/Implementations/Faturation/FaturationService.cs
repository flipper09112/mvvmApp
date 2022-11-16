using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.WebServices.Clients;
using tabApp.Core.Services.Implementations.WebServices.Sells;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.Services.Interfaces.WebServices.Admin;
using tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Clients;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Implementations.Faturation
{
    public class FaturationService : IFaturationService
    {
        public static string BaseUrl = "https://facturalusa.pt/api/v1";
        public static string APIKEY = "32Qv1lemMwEZtzk5zhVyHSRGbyiCMiVRjP99MLdOSruZyVop2DPEXaexUXpa2HmIIGutiRxu8vBhTprdHnijPh2GbjC1Py79BBlVryGPqSJebN2geaKO1LBySWezYVER";

        private readonly IGetVendasListaRequest _getVendasListaRequest;
        private readonly IDialogService _dialogService;
        private readonly IGetVehiclesRequest _getVehiclesRequest;
        private readonly IGetClientRequest _getClientRequest;
        private readonly ICreateSellDocumentRequest _createSellDocumentRequest;

        public TrasnportationDoc DocumentSelected { get; set; }

        public FaturationService(IGetVendasListaRequest getVendasListaRequest,
                                 IDialogService dialogService,
                                 IGetVehiclesRequest getVehiclesRequest,
                                 IGetClientRequest getClientRequest,
                                 ICreateSellDocumentRequest createSellDocumentRequest)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _getVehiclesRequest = getVehiclesRequest;
            _getClientRequest = getClientRequest;
            _createSellDocumentRequest = createSellDocumentRequest;
        }

        private TrasnportationsDocs _trasnportationsDocs;

        public TrasnportationsDocs TrasnportationsDocs
        {
            get {
                if (_trasnportationsDocs == null)
                    _trasnportationsDocs = new TrasnportationsDocs(_getVendasListaRequest, _dialogService, _createSellDocumentRequest);
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

        public TrasnportationsDocs(IGetVendasListaRequest getVendasListaRequest, 
                                   IDialogService dialogService,
                                   ICreateSellDocumentRequest createSellDocumentRequest)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _createSellDocumentRequest = createSellDocumentRequest;
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
    }
}
