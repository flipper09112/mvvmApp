﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.Clients;
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
        public static string APIKEY = "ZnrkWoPMFvsMmLUujyWDSPjhWZ2HvxIa6uRNVhuciqjXF12WhcSrlKgDG0MUfIK8muqCP32RrOuHbxgWHRaHazicKzl1zrWA82DTWozofhPKwnQ0GyXP9PKaV3MB7IQ0";
        //public static string APIKEY = "vvrwuk2AP4mAjNnliYav0n9lNvkZ5AbUVOdGFeQsDijeFtVkw3asV9W7Kr2Cg0V5s8M65xJrl3y9g9aTMLcrXJ0qMMyiI8MINtEx0cESne6zC0YylSpL9ln3J6M9rNwv";

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
                                 IClientsManagerService clientsManagerService)
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
                                                _deleteFatProductRequest);
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
                    findProduct.quantity -= product.quantity;
                }
            }

            itemsRemaining = itemsRemaining.Where(item => item.quantity > 0).ToList();

            itemsRemaining.ForEach(product => fatItems.Add(new FatItem()
            {
                Id = product.item.reference.ToString(),
                Details = product.item_details,
                Discount = product.discount.ToString(),
                Price = _productsManagerService.GetProductById(int.Parse(product.item.reference)).PVP.ToString(),
                Vat = product?.vat?.tax.ToString() ?? "NaN",
                Quantity = product.quantity.ToString(),
            }));

            return fatItems;
        }
    }
}