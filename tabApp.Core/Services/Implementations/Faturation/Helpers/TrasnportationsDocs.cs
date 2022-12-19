using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using System.Linq;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace tabApp.Core.Services.Implementations.Faturation.Helpers
{
    public class TrasnportationsDocs
    {
        private IGetVendasListaRequest _getVendasListaRequest { get; }
        private IDialogService _dialogService;
        private ICreateSellDocumentRequest _createSellDocumentRequest;
        private IDownloadFatRequest _downloadFatRequest;
        private IDuplicateWayBillRequest _duplicateWayBillRequest;
        private IUpdateFatRequest _updateFatRequest;
        private FatProducts _fatProductsManager;
        private Clients _fatClientsManager;

        public TrasnportationsDocs(IGetVendasListaRequest getVendasListaRequest,
                                   IDialogService dialogService,
                                   ICreateSellDocumentRequest createSellDocumentRequest,
                                   IDownloadFatRequest downloadFatRequest,
                                   IDuplicateWayBillRequest duplicateWayBillRequest,
                                   IUpdateFatRequest updateFatRequest,
                                   FatProducts products,
                                   Clients fatClientsManager)
        {
            _getVendasListaRequest = getVendasListaRequest;
            _dialogService = dialogService;
            _createSellDocumentRequest = createSellDocumentRequest;
            _downloadFatRequest = downloadFatRequest;
            _duplicateWayBillRequest = duplicateWayBillRequest;
            _updateFatRequest = updateFatRequest;
            _fatProductsManager = products;
            _fatClientsManager = fatClientsManager;
        }


        public async Task<List<TrasnportationDoc>> GetVendasLista(SellsTypes sellType, int? id = null)
        {
            List<TrasnportationDoc> trasnportationDocs = new List<TrasnportationDoc>();
            var response = await _getVendasListaRequest.SendAsync(new GetVendasListaInput()
            {
                Type = sellType
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return trasnportationDocs;
            }

            if(id != null)
            {
                response.Data.Where(fat => fat.customer.code == id.ToString()).ToList().ForEach(doc => trasnportationDocs.Add(new TrasnportationDoc()
                {
                    ID = doc.id,
                    Name = doc.documenttype.saft_initials + "-" + doc.serie.description + "-" + doc.documenttypeserie.document_number,
                    DocumentUrl = doc.url_file,
                    ProductItems = doc.items,
                    StartTravelDate = doc.waybill_shipping_date == null ? DateTime.MinValue : DateTime.Parse(doc.waybill_shipping_date),
                    EmissionDate = doc.file_last_generated == null ? DateTime.MinValue : DateTime.Parse(doc.file_last_generated)
                }));
            }
            else
            {
                response.Data.ForEach(doc => trasnportationDocs.Add(new TrasnportationDoc()
                {
                    ID = doc.id,
                    Name = doc.documenttype.saft_initials + "-" + doc.serie.description + "-" + doc.documenttypeserie.document_number,
                    DocumentUrl = doc.url_file,
                    ProductItems = doc.items,
                    StartTravelDate = doc.waybill_shipping_date == null ? DateTime.MinValue : DateTime.Parse(doc.waybill_shipping_date),
                    EmissionDate = doc.file_last_generated == null ? DateTime.MinValue : DateTime.Parse(doc.file_last_generated)
                }));
            }

            return trasnportationDocs;
        }

        public async Task<TrasnportationDoc> CreateDocument(FatClient fatClient, Car vehicleSelected, DateTime dateSelected, List<FatItem> productsList)
        {
            var success = await _fatProductsManager.ValidateItems(productsList);

            if (!success) return null;

            var response = await _createSellDocumentRequest.SendAsync(new CreateSellDocumentInput()
            {
                IssueDate = DateTime.Now,
                DocumentType = DocumentTypeEnum.GuiadeTransporte,
                CustomerId = fatClient.Id.ToString(),
                //VatNumber = clientSelected.NIF,
                //Address = clientSelected.Address,
                //City = clientSelected.City,
                //PostalCode = clientSelected.PostalCode,
                //Country = clientSelected.Country,
                VatType = VatTypeEnum.Nãofazernada,
                Vehicle = vehicleSelected.Id.ToString(),
                WaybillShippingDate = dateSelected,
                WaybillGlobal = true,
                //LocationOrigin = "Rua da Ciranda nº54, Santa Comba, Ponte de Lima, Portugal, 4990-740 Pte. de Lima",
                CargoLocation = "Ponte de Lima",
                Items = productsList,
                Status = StatusEnum.Terminado
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return null;
            }

            var docs = await GetVendasLista(SellsTypes.Guias);
            return docs[0];
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

        internal async Task<string> CreateFatByWayBill(TrasnportationDoc guiaSelected)
        {
            var response = await _duplicateWayBillRequest.SendAsync(new DuplicateWayBillInput()
            {
                Id = guiaSelected.ID.ToString(),
                DocumentType = DocumentTypeEnum.FacturaSimplificada
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return null;
            }

            return response.id.ToString();
        }

        internal async Task<TrasnportationDoc> UpdateFatDocument(string fatId, FatClient client, List<FatItem> productsList, TrasnportationDoc guiaSelected, bool faturaRecibo)
        {
            FatClient fatClient;

            if (client.Name.Equals("Consumidor final"))
                fatClient = client;
            else
                fatClient = await _fatClientsManager.ValidateClient(client);

            if (fatClient == null)
                return null;

            var response = await _updateFatRequest.SendAsync(new CreateSellDocumentInput()
            {
                Id = fatId,
                IssueDate = DateTime.Now,
                DocumentType = client.Name.Equals("Consumidor final") ? 
                                    DocumentTypeEnum.FacturaSimplificada :
                                    faturaRecibo ? 
                                        DocumentTypeEnum.FacturaRecibo :
                                        DocumentTypeEnum.Factura,
                CustomerId = fatClient.Id.ToString(),
                VatNumber = fatClient.NIF,
                Address = fatClient.Address,
                City = fatClient.City,
                PostalCode = fatClient.PostalCode,
                Country = fatClient.Country,
                WaybillShippingDate = guiaSelected.StartTravelDate,
                VatType = VatTypeEnum.IVAincluído,
                Items = productsList,
                Status = StatusEnum.Terminado,
                PaymentMethod = PaymentMethodEnum.Numerario
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return null;
            }

            return (await GetVendasLista(SellsTypes.Facturação))[0];
        }

    }
}
