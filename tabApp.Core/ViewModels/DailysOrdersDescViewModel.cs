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
        public List<(string ProductName, string Ammount)> SegDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach(var item in Client.SegDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if(product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> TerDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.TerDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> QuaDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.QuaDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> QuiDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.QuiDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> SexDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.SexDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> SabDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.SabDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        public List<(string ProductName, string Ammount)> DomDailyItemsList
        {
            get
            {
                List<(string ProductName, string Ammount)> items = new List<(string ProductName, string Ammount)>();

                Product product;

                foreach (var item in Client.DomDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        items.Add((product.Name, item.Ammount.ToString("N0")));
                    else
                        items.Add((product.Name, item.Ammount.ToString("N3")));
                }

                return items;
            }
        }
        #endregion
        public override void Appearing()
        {
        }
    }
}
