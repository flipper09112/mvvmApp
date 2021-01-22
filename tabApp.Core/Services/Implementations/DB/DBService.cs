using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Enums;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations
{
    public class DBService : IDBService
    {
        private readonly string ClientsListFileName = "MeuArquivoXLS.xls";
        private readonly string ProductsListFileName = "tabela_precos.xls";

        private readonly IGetFileService _getFileService;
        private readonly IFileService _fileService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IProductsManagerService _productsManagerService;

        public DBService(IProductsManagerService productsManagerService, 
            IClientsManagerService clientsManagerService, 
            IFileService fileService, 
            IGetFileService getFileService)
        {
            _fileService = fileService;
            _getFileService = getFileService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
        }

        public async Task StartAsync()
        {
            if (!_fileService.HasFile(ClientsListFileName))
            {
                _fileService.SaveFile(ClientsListFileName, await _getFileService.GetUrlDownload(ClientsListFileName));
                StartAsync();
                return;
            } else if(!_fileService.HasFile(ProductsListFileName))
            {
                _fileService.SaveFile(ProductsListFileName, await _getFileService.GetUrlDownload(ProductsListFileName));
                StartAsync();
                return;
            }
            else
            {
                byte[] byteArrayClients = _fileService.GetFile(ClientsListFileName);
                ReadClientsList(byteArrayClients);
                byte[] byteArrayProducts = _fileService.GetFile(ProductsListFileName);
                ReadProductsList(byteArrayProducts);
            }
        }

        private void ReadProductsList(byte[] byteArrayProducts)
        {
            MemoryStream ms = new MemoryStream(byteArrayProducts);

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            #region private fields
            List<Product> productsList = new List<Product>();
            string name;
            int id;
            string imageReference;
            bool unity;
            bool idConverted;
            ProductTypeEnum productType;
            double pvp;
            List<(int Id, double Value)> reSaleValues = new List<(int Id, double Value)>();
            #endregion

            for (int i = 2; i < sheet.Rows.Length; i++)
            {
                idConverted = int.TryParse(sheet.Range[i, (int)ProductsListItemsPositions.ID].DisplayedText, out id);
                if (!idConverted) break;
                name = sheet.Range[i, (int)ProductsListItemsPositions.Name].DisplayedText;
                imageReference = sheet.Range[i, (int)ProductsListItemsPositions.Ref].DisplayedText;
                unity = sheet.Range[i, (int)ProductsListItemsPositions.Ref].DisplayedText.Equals("1");
                productType = GetProductType(sheet.Range[i, (int)ProductsListItemsPositions.Type].DisplayedText);
                pvp = double.Parse(sheet.Range[i, (int)ProductsListItemsPositions.PVP].DisplayedText);

                productsList.Add(new Product(
                    name,
                    id,
                    imageReference,
                    unity,
                    productType,
                    pvp
                    ));
            }

            _productsManagerService.SetProducts(productsList);
        }

        private ProductTypeEnum GetProductType(string text)
        {
            if (text.Equals("1"))
                return ProductTypeEnum.Padaria;
            if (text.Equals("2"))
                return ProductTypeEnum.PastelariaIndividual;
            if (text.Equals("3"))
                return ProductTypeEnum.Outros;
            if (text.Equals("4"))
                return ProductTypeEnum.PastelariaIndividualSalgada;
            if (text.Equals("5"))
                return ProductTypeEnum.SemiFrioIndividual;
            if (text.Equals("6"))
                return ProductTypeEnum.SemiFrioFamiliar;
            if (text.Equals("7"))
                return ProductTypeEnum.BolosTradicionais;
            if (text.Equals("8"))
                return ProductTypeEnum.Sortido;
            if (text.Equals("9"))
                return ProductTypeEnum.Tartes;
            if (text.Equals("10"))
                return ProductTypeEnum.Tortas;
            if (text.Equals("11"))
                return ProductTypeEnum.BolosFestivos;

            return ProductTypeEnum.None;
        }

        public void ReadClientsList(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            #region params inputs
            int id;
            string name;
            string subName;
            string addressDesc;
            int door;
            string coord;
            Address address;
            DateTime paymentDate;
            string type;
            PaymentTypeEnum paymentType;
            bool active;
            double extraValueToPay;
            List<Client> clientsList = new List<Client>();
            List<DailyOrder> dailyOrdersList = new List<DailyOrder>();
            DailyOrder segDesc;
            DailyOrder terDesc;
            DailyOrder quaDesc;
            DailyOrder quiDesc;
            DailyOrder sexDesc;
            DailyOrder sabDesc;
            DailyOrder domDesc;
            #endregion

            for (int i = 2; i < sheet.Rows.Length; i++)
            {
                
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].Text);
                door = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Door].Text);
                extraValueToPay = double.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Extra].Text.Replace(".", ","));
                name = sheet.Range[i, (int)ClientsListItemsPositions.Name].Text;
                subName = sheet.Range[i, (int)ClientsListItemsPositions.SubName].Text;
                addressDesc = sheet.Range[i, (int)ClientsListItemsPositions.AddressDesc].Text;
                coord = sheet.Range[i, (int)ClientsListItemsPositions.Coordinates].Text;
                paymentDate = DateTime.ParseExact(sheet.Range[i, (int)ClientsListItemsPositions.Payment].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                type = sheet.Range[i, (int)ClientsListItemsPositions.PaymentType].Text;
                active = sheet.Range[i, (int)ClientsListItemsPositions.Active].Text.Equals("1");

                #region Daily Orders
                segDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.SegDes].Text, DayOfWeek.Monday);
                terDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.TerDes].Text, DayOfWeek.Tuesday);
                quaDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.QuaDes].Text, DayOfWeek.Wednesday);
                quiDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.QuiDes].Text, DayOfWeek.Thursday);
                sexDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.SexDes].Text, DayOfWeek.Friday);
                sabDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.SabDes].Text, DayOfWeek.Saturday);
                domDesc = GetOrder(sheet.Range[i, (int)ClientsListItemsPositions.DomDes].Text, DayOfWeek.Sunday);

                dailyOrdersList = new List<DailyOrder>();
                dailyOrdersList.Add(segDesc);
                dailyOrdersList.Add(terDesc);
                dailyOrdersList.Add(quaDesc);
                dailyOrdersList.Add(quiDesc);
                dailyOrdersList.Add(sexDesc);
                dailyOrdersList.Add(sabDesc);
                dailyOrdersList.Add(domDesc);
                #endregion

                address = new Address(addressDesc, door, coord);
                paymentType = GetPaymentType(type);

                clientsList.Add(new Client(id, name, subName, address, paymentDate, paymentType, active, extraValueToPay, dailyOrdersList));
            }
            _clientsManagerService.SetClients(clientsList);
        }

        private DailyOrder GetOrder(string orderDesc, DayOfWeek day)
        {
            List<(int ProductId, double Ammount)> allItems = new List<(int ProductId, double Ammount)>();

            int produtoId;
            double quantidade;

            foreach (string array in orderDesc.Split(';'))
            {
                if (array.Equals("-") || array.Equals(""))
                    break;

                produtoId = int.Parse(array.Split('-')[0]);
                quantidade = double.Parse(array.Split('-')[1]);

                allItems.Add((produtoId, quantidade));
            }

            return new DailyOrder(day, allItems);
        }

        private PaymentTypeEnum GetPaymentType(string type)
        {
            if (type.Equals("D"))
                return PaymentTypeEnum.Diario;
            if (type.Equals("S"))
                return PaymentTypeEnum.Semanal;
            if (type.Equals("M"))
                return PaymentTypeEnum.Mensal;
            if (type.Equals("JD"))
                return PaymentTypeEnum.JuntaDias;
            if (type.Equals("LS"))
                return PaymentTypeEnum.Loja;

            return PaymentTypeEnum.None;
        }
    }
}
