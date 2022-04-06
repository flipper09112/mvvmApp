using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class ReportViewModel : BaseViewModel
    {
        private IClientsManagerService _clientsManagerService;
        private IFileService _fileService;
        private IProductsManagerService _productsManagerService;
        private IDialogService _dialogService;
        private IMvxNavigationService _navigationService;
        private IAddProductToOrderService _addProductToOrderService;

        public bool AllDays { get; set; }
        public DateTime? DateSelected { get; set; }
        public List<Product> SelectedProducts { get; set; }

        public EventHandler OpenExternalApp;
        public EventHandler RemoveProductEvent;

        public MvxCommand GenerateNewFileCommand;
        public MvxCommand SelectDayCommand;
        public MvxCommand OpenPDFExtrenalAppCommand;
        public MvxCommand AddProductCommand;
        public MvxCommand<Product> RemoveProductCommand;

        #region Parameters
        public byte[] _reportArray;
        public byte[] ReportArray
        {
            get
            {
                return _reportArray;
            }
            set
            {
                _reportArray = value;
                GenerateNewFileCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ReportArray));
            }
        }
        #endregion

        public ReportViewModel(IClientsManagerService clientsManagerService,
                               IFileService fileService,
                               IProductsManagerService productsManagerService,
                               IDialogService dialogService,
                               IMvxNavigationService navigationService,
                               IAddProductToOrderService addProductToOrderService
                               )
        {
            _clientsManagerService = clientsManagerService;
            _fileService = fileService;
            _productsManagerService = productsManagerService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _addProductToOrderService = addProductToOrderService;

            GenerateNewFileCommand = new MvxCommand(GenerateNewFile, CanGenerateNewFile);
            SelectDayCommand = new MvxCommand(SelectDay);
            OpenPDFExtrenalAppCommand = new MvxCommand(OpenPDFExtrenalApp);
            AddProductCommand = new MvxCommand(AddProduct);
            RemoveProductCommand = new MvxCommand<Product>(RemoveProduct);

            SelectedProducts = new List<Product>();
            DateSelected = null;
        }

        private void RemoveProduct(Product product)
        {
            int position = SelectedProducts.IndexOf(product);
            SelectedProducts.Remove(product);

            RemoveProductEvent?.Invoke(position, null);
        }

        private async void AddProduct()
        {
            await _navigationService.Navigate<ChooseProductViewModel>();
        }

        private void OpenPDFExtrenalApp()
        {
            var f = _fileService.SaveFile("Report.pdf", ReportArray.ToArray(), true);
            OpenExternalApp?.Invoke(f, null);
        }

        private void SelectDay()
        {
            _dialogService.ShowDatePickerDialog(SelectDate, false);
        }

        private void SelectDate(DateTime obj)
        {
            DateSelected = obj;
            RaisePropertyChanged(nameof(DateSelected));
        }

        private bool CanGenerateNewFile()
        {
            if (AllDays) return true;
            else
            {
                if(DateSelected != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void GenerateNewFile()
        {
            IsBusy = true;
            CreateReport();
            IsBusy = false;
        }

        private void CreateReport()
        {
            var s = new MemoryStream();
            PdfDocument pdf = new PdfDocument(new PdfWriter(s));
            Document document = new Document(pdf);

            if(AllDays)
            {
                AllDaysReport(document);
            }
            else
            {
                DayReport(document);
            }

            document.Close();

            ReportArray = s.ToArray();

        }

        private void DayReport(Document document)
        {
            Paragraph header = new Paragraph(DateSelected?.ToString("dddd"))
                                           .SetTextAlignment(TextAlignment.CENTER)
                                           .SetFontSize(20);
            document.Add(header);

            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            Table table = new Table(2, true);
            foreach (var client in _clientsManagerService.ClientsList)
            {
                var todayOrder = _clientsManagerService.GetTodayDailyOrder(client, (DayOfWeek)DateSelected?.DayOfWeek);

                //aply filter here
                if(SelectedProducts.Count > 0)
                {
                    if (!ContainsProductFilter(todayOrder)) continue;
                }

                if (todayOrder.AllItems.Count == 0)
                {
                    table.AddCell(new Paragraph(client.Name + " - Id: " + client.Id.ToString()).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Paragraph("-"));
                }
                else
                {
                    table.AddCell(new Paragraph(client.Name + " - Id: " + client.Id.ToString())
                        .SetTextAlignment(TextAlignment.CENTER));

                    Product product;
                    string label = "";
                    foreach (var item in todayOrder.AllItems)
                    {
                        product = _productsManagerService.GetProductById(item.ProductId);

                        if (product.Unity)
                            label += product.Name + " - " + item.Ammount.ToString("N0");
                        else
                            label += product.Name + " - " + item.Ammount.ToString("N3");

                        if (item != todayOrder.AllItems.Last()) label += "\n";
                    }
                    table.AddCell(label);
                }
            }
            document.Add(table);
        }

        private bool ContainsProductFilter(DailyOrder todayOrder)
        {
            foreach(Product product in SelectedProducts)
            {
                foreach(var item in todayOrder.AllItems)
                {
                    if (item.ProductId == product.Id) return true;
                }
            }
            return false;
        }

        private void AllDaysReport(Document document)
        {
            DateTime date = DatesHelpers.GetFirstDayOfWeek(DateTime.Today);
            do
            {
                Paragraph header = new Paragraph(date.ToString("dddd"))
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetFontSize(20);
                document.Add(header);

                LineSeparator ls = new LineSeparator(new SolidLine());
                document.Add(ls);

                Table table = new Table(2, true);
                foreach (var client in _clientsManagerService.ClientsList)
                {
                    var todayOrder = _clientsManagerService.GetTodayDailyOrder(client, date.DayOfWeek);

                    if(todayOrder.AllItems.Count == 0)
                    {
                        table.AddCell(new Paragraph(client.Name + " - Id: " + client.Id.ToString()).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Paragraph("-"));
                    } else
                    {
                        table.AddCell(new Paragraph(client.Name + " - Id: " + client.Id.ToString())
                            .SetTextAlignment(TextAlignment.CENTER));

                        Product product;
                        string label = "";
                        foreach (var item in todayOrder.AllItems)
                        {
                            product = _productsManagerService.GetProductById(item.ProductId);

                            if (product.Unity)
                                label += product.Name + " - " + item.Ammount.ToString("N0");
                            else
                                label += product.Name + " - " + item.Ammount.ToString("N3");

                            if (item != todayOrder.AllItems.Last()) label += "\n";
                        }
                        table.AddCell(label);
                    }
                }
                document.Add(table);
                date = date.AddDays(1);
            }
            while (date.DayOfWeek != DayOfWeek.Monday);
        }

        public override void Appearing()
        {
            if(_addProductToOrderService.ProductsSelected?.Count > 0)
            {
                SelectedProducts.Add(_addProductToOrderService.ProductsSelected[0]);
                _addProductToOrderService.Clear();
            }
        }

        public override void DisAppearing()
        {
        }
    }
}
