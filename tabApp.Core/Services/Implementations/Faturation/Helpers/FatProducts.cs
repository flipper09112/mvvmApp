using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.Services.Implementations.Products;
using System.Linq;
using tabApp.Core.Services.Implementations.WebServices.Products;
using Org.BouncyCastle.Math;
using static Common.Logging.Configuration.ArgUtils;
using tabApp.Core.Services.Implementations.DB;

namespace tabApp.Core.Services.Implementations.Faturation.Helpers
{
    public class FatProducts
    {
#if DEBUG
        private const string PriceTableId = "61132"; //dev
#elif RELEASE
        private const string PriceTableId = "61451"; //prod
#endif

        public IDialogService _dialogService { get; }

        private IAddFatProductRequest _addFatProductRequest { get; }

        private IGetFatProductRequest _getFatProductRequest;
        private IProductsManagerService _productsManagerService;
        private IUpdateFatProductRequest _updateFatProductRequest;
        private IDeleteFatProductRequest _deleteFatProductRequest;
        private IDataBaseManagerService _dataBaseManagerService;

        public FatProducts(IDialogService dialogService, 
                           IAddFatProductRequest addFatProductRequest,
                           IGetFatProductRequest getFatProductRequest,
                           IProductsManagerService productsManagerService,
                           IUpdateFatProductRequest updateFatProductRequest,
                           IDeleteFatProductRequest deleteFatProductRequest,
                           IDataBaseManagerService dataBaseManagerService)
        {
            _dialogService = dialogService;
            _addFatProductRequest = addFatProductRequest;
            _getFatProductRequest = getFatProductRequest;
            _productsManagerService = productsManagerService;
            _updateFatProductRequest = updateFatProductRequest;
            _deleteFatProductRequest = deleteFatProductRequest;
            _dataBaseManagerService = dataBaseManagerService;
        }

        internal async Task<int> AddProduct(Product product)
        {
            var price = new List<FatPrices>()
            {
                new FatPrices()
                {
                    Id = PriceTableId,
                    Price = product.PVP,
                    Discount = 0
                }
            };

            var response = await _addFatProductRequest.SendAsync(new AddFatProductInput()
            {
                Reference = product.Id.ToString(),
                Description = product.Name,
                Unit = product.Unity ? "uni" : "kg",
                Vat = product.Iva,
                Type = FatProductTypeEnum.Mercadorias,
                Prices = JsonConvert.SerializeObject(price),
                DetailsShowPrint = false,

            });

            if (!response.Success)
            {
                Debug.WriteLine(response.Error);
                return -1;
            }

            return response.data.id;
        }

        internal async Task<bool> ValidateItems(List<FatItem> productsList)
        {
            //TODO delete this for

            foreach (var item in productsList)
            {
                var response = await _getFatProductRequest.SendAsync(new GetFatProductInput()
                {
                    Value= item.Id,
                    SearchIn = FatProductPropertyEnum.Reference
                });

                if (!response.Success)
                {
                    _dialogService.ShowErrorDialog(string.Empty, response.Error);
                    return false;
                }

                var product = _productsManagerService.GetProductById(int.Parse(item.Id));
                if (product.Iva == 0 && product.IsencaoIva == "M18")
                {
                    _dialogService.ShowErrorDialog(string.Empty, "Produto sem dados sobre o IVA");
                    return false;
                }

                //criar produto senao existir
                if (response.data.Count == 0)
                {
                    var id = await AddProduct(product);
                    //await UpdateProduct(id, product);
                }
                //update product if price is different
                else if(response.data.First().prices.Count == 0 ||
                   response.data.First().prices.Find(table => table.price_id.ToString() == PriceTableId).price != product.PVP)
                {
                    await UpdateProduct(response.data.First().id, product);
                }

                //Cabaz alimentar
                if(DateTime.Now.Date == new DateTime(2023, 04, 18).Date)
                {
                    if(product.Iva == 6)
                    {
                        product.Iva = 0;
                        product.IsencaoIva = "M26";
                        _dataBaseManagerService.SaveProduct(product);
                        item.Vat = "0";
                        item.VatExemption = "M26";
                        await UpdateProduct(response.data.First().id, product);
                    }
                }

                //Fim Cabaz alimentar
                if (DateTime.Now.Date == new DateTime(2023, 11, 1).Date)
                {
                    if (product.Iva == 0 && product.IsencaoIva == "M26")
                    {
                        product.Iva = 6;
                        product.IsencaoIva = "M18";
                        _dataBaseManagerService.SaveProduct(product);
                        item.Vat = "6";
                        item.VatExemption = "M18";
                        await UpdateProduct(response.data.First().id, product);
                    }
                }

            }

            return true;
        }

        public async Task UpdateProduct(int id, Product product)
        {
            var price = new List<FatPrices>()
            {
                new FatPrices()
                {
                    Id = PriceTableId,
                    Price = product.PVP,
                    Discount = 0
                }
            };

            var response = await _updateFatProductRequest.SendAsync(new UpdateFatProductInput()
            {
                Id = id.ToString(),
                Reference = product.Id.ToString(),
                Description = product.Name,
                Unit = product.Unity ? "uni" : "kg",
                Vat = product.Iva,
                VatException = product.IsencaoIva ?? "M18",
                Type = FatProductTypeEnum.Mercadorias,
                Active = true,
                Prices = JsonConvert.SerializeObject(price),
                DetailsShowPrint = false
            });

            if (!response.Success || !response.status)
            {
                _dialogService.ShowErrorDialog(string.Empty, "Erro ao atualizar " + product.Name);
                return;
            }
        }
    }
}
