using Spire.Xls;
using System;
using System.Collections.Generic;
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
            }
            else
            {
                byte[] byteArray = _fileService.GetFile(ClientsListFileName);
                ReadClientsList(byteArray);
            }
        }

        public void ReadClientsList(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

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

            for(int i = 2; i < sheet.Rows.Length; i++)
            {
                
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].Text);
                door = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Door].Text);
                extraValueToPay = double.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Extra].Text.Replace(".", ","));
                name = sheet.Range[i, (int)ClientsListItemsPositions.Name].Text;
                subName = sheet.Range[i, (int)ClientsListItemsPositions.SubName].Text;
                addressDesc = sheet.Range[i, (int)ClientsListItemsPositions.AddressDesc].Text;
                coord = sheet.Range[i, (int)ClientsListItemsPositions.Coordinates].Text;
                paymentDate = DateTime.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Payment].Text);
                type = sheet.Range[i, (int)ClientsListItemsPositions.PaymentType].Text;
                active = sheet.Range[i, (int)ClientsListItemsPositions.Active].Text.Equals("1");

                address = new Address(addressDesc, door, coord);
                paymentType = GetPaymentType(type);

                System.Diagnostics.Debug.WriteLine(name);

                clientsList.Add(new Client(id, name, subName, address, paymentDate, paymentType, active, extraValueToPay));
            }

            _clientsManagerService.SetClients(clientsList);

        }

        private PaymentTypeEnum GetPaymentType(string type)
        {
            if (type.Equals("D"))
                return PaymentTypeEnum.Diario;
            if (type.Equals("S"))
                return PaymentTypeEnum.Diario;
            if (type.Equals("M"))
                return PaymentTypeEnum.Diario;
            if (type.Equals("JD"))
                return PaymentTypeEnum.Diario;
            if (type.Equals("LS"))
                return PaymentTypeEnum.Diario;

            return PaymentTypeEnum.None;
        }
    }
}
