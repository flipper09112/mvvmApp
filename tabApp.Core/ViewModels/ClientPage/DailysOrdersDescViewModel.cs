using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels
{
    public class DailysOrdersDescViewModel : BaseViewModel
    {

        private readonly IChooseClientService _chooseClientService;
        private readonly IProductsManagerService _productsManagerService;

        public DailysOrdersDescViewModel(IChooseClientService chooseClientService,
                                         IProductsManagerService productsManagerService)
        {
            _chooseClientService = chooseClientService;
            _productsManagerService = productsManagerService;
        }

        public Client Client => _chooseClientService.ClientSelected;

        #region Get Lists
        public List<(Product Product, string Ammount)> SegDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach(var item in Client.SegDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if(product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> TerDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.TerDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> QuaDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.QuaDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> QuiDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.QuiDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> SexDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.SexDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> SabDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.SabDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(Product Product, string Ammount)> DomDailyItemsList
        {
            get
            {
                List<(Product Product, string Ammount)> items = new List<(Product Product, string Ammount)>();

                Product product;

                foreach (var item in Client.DomDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product, item.Ammount.ToString("N0")));
                    else
                        items.Add((product, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        #endregion
        public override void Appearing()
        {
        }
        public override void DisAppearing()
        {
        }
    }
}
