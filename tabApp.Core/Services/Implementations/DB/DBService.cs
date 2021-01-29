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
        private readonly string LogsListFileName = "logs.xls";

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

        public void SaveNewClientData(Client client)
        {
            byte[] byteArrayClients = _fileService.GetFile(ClientsListFileName);
            byte[] byteArrayClientsUpdated = UpdateClientDB(client, byteArrayClients);
            _fileService.SaveFile(ClientsListFileName, byteArrayClientsUpdated, true);
        }
        public void SaveNewRegist(Regist regist)
        {
            byte[] byteArrayLogs = _fileService.GetFile(LogsListFileName);
            byte[] byteArrayLogsUpdated = NewRegistsDB(regist, byteArrayLogs);
            _fileService.SaveFile(LogsListFileName, byteArrayLogsUpdated, true);
        }
        public void SaveNewRegist(ExtraOrder order)
        {
            byte[] byteArrayLogs = _fileService.GetFile(LogsListFileName);
            byte[] byteArrayLogsUpdated = NewRegistsDB(order, byteArrayLogs);
            _fileService.SaveFile(LogsListFileName, byteArrayLogsUpdated, true);
        }

        public async Task StartAsync()
        {
            if (!_fileService.HasFile(ClientsListFileName))
            {
                _fileService.SaveFile(ClientsListFileName, await _getFileService.GetUrlDownload(ClientsListFileName));
                await StartAsync();
                return;
            } else if(!_fileService.HasFile(ProductsListFileName))
            {
                _fileService.SaveFile(ProductsListFileName, await _getFileService.GetUrlDownload(ProductsListFileName));
                await StartAsync();
                return;
            }
            else if (!_fileService.HasFile(LogsListFileName))
            {
                _fileService.SaveFile(LogsListFileName, await _getFileService.GetUrlDownload(LogsListFileName));
                await StartAsync();
                return;
            }
            else
            {
                byte[] byteArrayClients = _fileService.GetFile(ClientsListFileName);
                ReadClientsList(byteArrayClients);
                byte[] byteArrayProducts = _fileService.GetFile(ProductsListFileName);
                ReadProductsList(byteArrayProducts);
                byte[] byteArrayLogs = _fileService.GetFile(LogsListFileName);
                ReadLogsList(byteArrayLogs);
            }
        }

        #region Read Functions
        private void ReadLogsList(byte[] byteArrayLogs)
        {
            MemoryStream ms = new MemoryStream(byteArrayLogs);

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            #region private params
            int clientId;
            string info;
            string extraOrderDesc;
            bool total;
            DetailTypeEnum detailType;
            DateTime detailDate;
            DateTime orderDate;
            ExtraOrder extraOrder;
            #endregion

            for (int i = 2; i <= sheet.Rows.Length; i++)
            {
                clientId = int.Parse(sheet.Range[i, (int)LogsListItemsPositions.ID].DisplayedText);
                detailType = GetLogType(sheet.Range[i, (int)LogsListItemsPositions.Type].DisplayedText);
                info = sheet.Range[i, (int)LogsListItemsPositions.Info].DisplayedText;
                detailDate = DateTime.Parse(sheet.Range[i, (int)LogsListItemsPositions.Date].DisplayedText);
                DateTime.TryParse(sheet.Range[i, (int)LogsListItemsPositions.OrderDay].DisplayedText, out orderDate);
                extraOrderDesc = sheet.Range[i, (int)LogsListItemsPositions.Order].DisplayedText;
                total = sheet.Range[i, (int)LogsListItemsPositions.IsAll].DisplayedText.Equals("1");

                if (detailType == DetailTypeEnum.Order)
                {
                    _clientsManagerService.SetNewOrder(
                        clientId,
                        new ExtraOrder(clientId, detailDate, orderDate, GetOrderListItems(extraOrderDesc), total)
                        );
                }
                else
                {
                    _clientsManagerService.SetNewRegist(
                        clientId,
                        new Regist(detailDate, info, clientId, detailType)
                    );
                }
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
            int clientId;
            double productValue;
            string imageReference;
            bool unity;
            bool idConverted;
            ProductTypeEnum productType;
            double pvp;
            List<(int Id, double Value)> reSaleValues = new List<(int Id, double Value)>();
            #endregion

            for (int i = 2; i <= sheet.Rows.Length; i++)
            {
                idConverted = int.TryParse(sheet.Range[i, (int)ProductsListItemsPositions.ID].DisplayedText, out id);
                if (!idConverted) break;
                name = sheet.Range[i, (int)ProductsListItemsPositions.Name].DisplayedText;
                imageReference = sheet.Range[i, (int)ProductsListItemsPositions.Ref].DisplayedText;
                unity = sheet.Range[i, (int)ProductsListItemsPositions.Unity].DisplayedText.Equals("1");
                productType = GetProductType(sheet.Range[i, (int)ProductsListItemsPositions.Type].DisplayedText);
                pvp = double.Parse(sheet.Range[i, (int)ProductsListItemsPositions.PVP].DisplayedText);

                reSaleValues = new List<(int Id, double Value)>();
                for (int k = (int)ProductsListItemsPositions.PVP + 1; k < sheet.Columns.Length; k++)
                {
                    idConverted = int.TryParse(sheet.Range[1, k].DisplayedText, out clientId);
                    if (!idConverted) break;
                    productValue = double.Parse(sheet.Range[i, k].DisplayedText.Replace(".", ","));

                    reSaleValues.Add((clientId, productValue));
                }

                productsList.Add(new Product(
                    name,
                    id,
                    imageReference,
                    unity,
                    productType,
                    pvp,
                    reSaleValues
                    ));
            }

            _productsManagerService.SetProducts(productsList);
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

            for (int i = 2; i <= sheet.Rows.Length; i++)
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
                segDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.SegDes].Text, DayOfWeek.Monday);
                terDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.TerDes].Text, DayOfWeek.Tuesday);
                quaDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.QuaDes].Text, DayOfWeek.Wednesday);
                quiDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.QuiDes].Text, DayOfWeek.Thursday);
                sexDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.SexDes].Text, DayOfWeek.Friday);
                sabDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.SabDes].Text, DayOfWeek.Saturday);
                domDesc = GetDailyOrder(sheet.Range[i, (int)ClientsListItemsPositions.DomDes].Text, DayOfWeek.Sunday);

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
        #endregion

        #region Update
        private byte[] UpdateClientDB(Client client, byte[] byteArrayClients)
        {
            MemoryStream ms = new MemoryStream(byteArrayClients);
            MemoryStream output = new MemoryStream();

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            int id;

            for (int i = 2; i <= sheet.Rows.Length; i++)
            {
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].Text);

                if(id == client.Id)
                {
                    sheet.Range[i, (int)ClientsListItemsPositions.Payment].Text = client.PaymentDate.ToString("dd/MM/yyyy");
                    sheet.Range[i, (int)ClientsListItemsPositions.Name].Text = client.Name;
                    sheet.Range[i, (int)ClientsListItemsPositions.SubName].Text = client.SubName;
                    sheet.Range[i, (int)ClientsListItemsPositions.AddressDesc].Text = client.Address.AddressDesc;
                    sheet.Range[i, (int)ClientsListItemsPositions.Door].Text = client.Address.NumberDoor.ToString();
                    sheet.Range[i, (int)ClientsListItemsPositions.Coordinates].Text = client.Address.Coordenadas;
                    sheet.Range[i, (int)ClientsListItemsPositions.PaymentType].Text = GetPaymentType(client.PaymentType);
                    sheet.Range[i, (int)ClientsListItemsPositions.Extra].Text = client.ExtraValueToPay.ToString();
                    sheet.Range[i, (int)ClientsListItemsPositions.Active].Text = client.Active ? "1" : "0";
                    sheet.Range[i, (int)ClientsListItemsPositions.SegDes].Text = GetOrderStringDb(client.SegDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.TerDes].Text = GetOrderStringDb(client.TerDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.QuaDes].Text = GetOrderStringDb(client.QuaDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.QuiDes].Text = GetOrderStringDb(client.QuiDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.SexDes].Text = GetOrderStringDb(client.SexDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.SabDes].Text = GetOrderStringDb(client.SabDailyOrder);
                    sheet.Range[i, (int)ClientsListItemsPositions.DomDes].Text = GetOrderStringDb(client.DomDailyOrder);
                    break;
                }
            }

            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }
        private byte[] NewRegistsDB(Regist regist, byte[] byteArrayRegists)
        {
            MemoryStream ms = new MemoryStream(byteArrayRegists);
            MemoryStream output = new MemoryStream();

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            int newRow = sheet.Rows.Length + 1;
            sheet.InsertRow(newRow);

            sheet.Range[newRow, (int)LogsListItemsPositions.ID].Text = regist.ClientId.ToString();
            sheet.Range[newRow, (int)LogsListItemsPositions.Type].Text = regist.DetailType.ToString();
            sheet.Range[newRow, (int)LogsListItemsPositions.Info].Text = regist.Info;
            sheet.Range[newRow, (int)LogsListItemsPositions.Date].Text = regist.DetailRegistDay.ToString("dd/MM/yyyy");

            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }
        private byte[] NewRegistsDB(ExtraOrder regist, byte[] byteArrayRegists)
        {
            MemoryStream ms = new MemoryStream(byteArrayRegists);
            MemoryStream output = new MemoryStream();

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            int newRow = sheet.Rows.Length + 1;
            sheet.InsertRow(newRow);

            sheet.Range[newRow, (int)LogsListItemsPositions.ID].Text = regist.ClientId.ToString();
            sheet.Range[newRow, (int)LogsListItemsPositions.Type].Text = regist.DetailType.ToString();
            sheet.Range[newRow, (int)LogsListItemsPositions.Info].Text = string.Empty;
            sheet.Range[newRow, (int)LogsListItemsPositions.Date].Text = regist.OrderRegistDay.ToString("dd/MM/yyyy");
            sheet.Range[newRow, (int)LogsListItemsPositions.OrderDay].Text = regist.OrderDay.ToString("dd/MM/yyyy");
            sheet.Range[newRow, (int)LogsListItemsPositions.Order].Text = GetOrderStringDb(regist.AllItems);
            sheet.Range[newRow, (int)LogsListItemsPositions.IsAll].Text = regist.IsTotal ? "1" : "0";

            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }
        #endregion

        #region Delete
        public void RemoveRegist(ExtraOrder obj)
        {
            byte[] byteArrayLogs = _fileService.GetFile(LogsListFileName);
            byte[] byteArrayLogsUpdated = DeleteRegistsDB(obj, byteArrayLogs);
            _fileService.SaveFile(LogsListFileName, byteArrayLogsUpdated, true);

        }
        private byte[] DeleteRegistsDB(ExtraOrder obj, byte[] byteArrayLogs)
        {
            MemoryStream ms = new MemoryStream(byteArrayLogs);
            MemoryStream output = new MemoryStream();

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            #region private params
            int clientId;
            string info;
            string extraOrderDesc;
            bool total;
            DetailTypeEnum detailType;
            DateTime detailDate;
            DateTime orderDate;
            ExtraOrder extraOrder;
            #endregion

            for (int i = 2; i <= sheet.Rows.Length; i++)
            {
                clientId = int.Parse(sheet.Range[i, (int)LogsListItemsPositions.ID].DisplayedText);
                detailType = GetLogType(sheet.Range[i, (int)LogsListItemsPositions.Type].DisplayedText);
                info = sheet.Range[i, (int)LogsListItemsPositions.Info].DisplayedText;
                detailDate = DateTime.Parse(sheet.Range[i, (int)LogsListItemsPositions.Date].DisplayedText);
                DateTime.TryParse(sheet.Range[i, (int)LogsListItemsPositions.OrderDay].DisplayedText, out orderDate);
                extraOrderDesc = sheet.Range[i, (int)LogsListItemsPositions.Order].DisplayedText;
                total = sheet.Range[i, (int)LogsListItemsPositions.IsAll].DisplayedText.Equals("1");

                if (detailType == DetailTypeEnum.Order)
                {

                    ExtraOrder order = new ExtraOrder(clientId, detailDate, orderDate, GetOrderListItems(extraOrderDesc), total);
                    if (order.Equals(obj))
                    {
                        sheet.DeleteRow(i);
                        break;
                    }
                }
            }
            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }
        #endregion

        #region HELPERS
        private DetailTypeEnum GetLogType(string displayedText)
        {
            if (DetailTypeEnum.Payment.ToString().Equals(displayedText))
            {
                return DetailTypeEnum.Payment;

            } else if (DetailTypeEnum.AddExtra.ToString().Equals(displayedText))
            {
                return DetailTypeEnum.AddExtra;
            }
            else if (DetailTypeEnum.Order.ToString().Equals(displayedText))
            {
                return DetailTypeEnum.Order;
            }
            else if (DetailTypeEnum.CancelOrder.ToString().Equals(displayedText))
            {
                return DetailTypeEnum.CancelOrder;
            }
            else
            {
                return DetailTypeEnum.None;
            }
        }
        private List<(int ProductId, double Ammount)> GetOrderListItems(string displayedText)
        {
            List<(int ProductId, double Ammount)> allItems = new List<(int ProductId, double Ammount)>();

            int produtoId;
            double quantidade;

            foreach (string array in displayedText.Split(';'))
            {
                if (array.Equals("-") || array.Equals(""))
                    break;

                produtoId = int.Parse(array.Split('-')[0]);
                quantidade = double.Parse(array.Split('-')[1]);

                allItems.Add((produtoId, quantidade));
            }

            return allItems;
        }
        private DailyOrder GetDailyOrder(string orderDesc, DayOfWeek day)
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
        private string GetOrderStringDb(DailyOrder segDailyOrder)
        {
            string order = "";
            bool first = true;

            if (segDailyOrder.AllItems.Count == 0)
                return "-";

            foreach(var item in segDailyOrder.AllItems)
            {
                if(first)
                {
                    first = false;
                    order += item.ProductId + "-" + item.Ammount;
                } else
                {
                    order += ";" + item.ProductId + "-" + item.Ammount;
                }

            }
            return order;
        }
        private string GetOrderStringDb(List<(int ProductId, double Ammount)> allItems)
        {
            string order = "";
            bool first = true;

            if (allItems.Count == 0)
                return "-";

            foreach (var item in allItems)
            {
                if (first)
                {
                    first = false;
                    order += item.ProductId + "-" + item.Ammount;
                }
                else
                {
                    order += ";" + item.ProductId + "-" + item.Ammount;
                }

            }
            return order;
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
        private string GetPaymentType(PaymentTypeEnum paymentType)
        {
            switch(paymentType)
            {
                case PaymentTypeEnum.Diario:
                    return "D";
                case PaymentTypeEnum.Semanal:
                    return "S";
                case PaymentTypeEnum.Mensal:
                    return "M";
                case PaymentTypeEnum.JuntaDias:
                    return "JD";
                case PaymentTypeEnum.Loja:
                    return "LS";
                default:
                    return "N";
            }
        }
        #endregion
    }
}
