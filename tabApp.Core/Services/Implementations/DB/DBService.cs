using Spire.Xls;
using System;
using System.Collections.Generic;
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

namespace tabApp.Core.Services.Implementations
{
    public class DBService : IDBService
    {
        private readonly string ClientsListFileName = "MeuArquivoXLS.xls";
        private readonly string ProductsListFileName = "tabela_precos.xls";

        private readonly IGetFileService _getFileService;
        private readonly IFileService _fileService;
        private readonly IClientsManagerService _clientsManagerService;

        public DBService(IClientsManagerService clientsManagerService, IFileService fileService, IGetFileService getFileService)
        {
            _fileService = fileService;
            _getFileService = getFileService;
            _clientsManagerService = clientsManagerService;
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
            int imageReference;
            bool unity;
            ProductTypeEnum productType;
            double pvp;
            List<(int Id, double Value)> reSaleValues = new List<(int Id, double Value)>();
            #endregion

            for (int i = 2; i < sheet.Rows.Length; i++)
            {
                id = int.Parse(sheet.Range[i, (int)ProductsListItemsPositions.ID].Text);
            }
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
            string segDesc;
            string terDesc;
            string quaDesc;
            string quiDesc;
            string sexDesc;
            string sabDesc;
            string domDesc;
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

                segDesc = sheet.Range[i, (int)ClientsListItemsPositions.SegDes].Text;
                terDesc = sheet.Range[i, (int)ClientsListItemsPositions.TerDes].Text;
                quaDesc = sheet.Range[i, (int)ClientsListItemsPositions.QuaDes].Text;
                quiDesc = sheet.Range[i, (int)ClientsListItemsPositions.QuiDes].Text;
                sexDesc = sheet.Range[i, (int)ClientsListItemsPositions.SexDes].Text;
                sabDesc = sheet.Range[i, (int)ClientsListItemsPositions.SabDes].Text;
                domDesc = sheet.Range[i, (int)ClientsListItemsPositions.DomDes].Text;

                address = new Address(addressDesc, door, coord);
                paymentType = GetPaymentType(type);

                clientsList.Add(new Client(id, name, subName, address, paymentDate, paymentType, active, extraValueToPay));
            }

            _clientsManagerService.SetClients(clientsList);

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
