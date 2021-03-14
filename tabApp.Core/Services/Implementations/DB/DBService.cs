using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
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
        private readonly string OldLogsListFileName = "logs.txt";

        private readonly IFirebaseService _firebaseService;
        private readonly IFileService _fileService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IProductsManagerService _productsManagerService;

        public DBService(IProductsManagerService productsManagerService, 
            IClientsManagerService clientsManagerService, 
            IFileService fileService, 
            IFirebaseService getFileService)
        {
            _fileService = fileService;
            _firebaseService = getFileService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
        }

        public void SaveAllDocs()
        {
            byte[] fileData = _fileService.GetFile(ClientsListFileName);
            _firebaseService.SaveFile(ClientsListFileName, fileData);
            fileData = _fileService.GetFile(ProductsListFileName);
            _firebaseService.SaveFile(ProductsListFileName, fileData);
            fileData = _fileService.GetFile(LogsListFileName);
            _firebaseService.SaveFile(LogsListFileName, fileData);
        }

        #region SaveDataFunctions
        public void SaveClientData(Client client)
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
        #endregion

        private bool firstTime = false;
        private bool firstTimeWrite = false;

        public async Task StartAsync()
        {
            if (!_fileService.HasFile(ClientsListFileName))
            {
                _fileService.SaveFile(ClientsListFileName, await _firebaseService.GetUrlDownload(ClientsListFileName));
                await StartAsync();
                return;
            } else if(!_fileService.HasFile(ProductsListFileName))
            {
                _fileService.SaveFile(ProductsListFileName, await _firebaseService.GetUrlDownload(ProductsListFileName));
                await StartAsync();
                return;
            }
            else if (!_fileService.HasFile(LogsListFileName))
            {
                _fileService.SaveFile(LogsListFileName, await _firebaseService.GetUrlDownload(LogsListFileName));
                await StartAsync();
                return;
            }
            else if (/*!_fileService.HasFile(LogsListFileName)*/firstTime)
            {
                firstTime = false;
                _fileService.SaveFile(OldLogsListFileName, await _firebaseService.GetUrlDownload(OldLogsListFileName));
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
                if(firstTimeWrite)
                {
                    byte[] byteArrayOldLogs = _fileService.GetFile(OldLogsListFileName);
                    ReadOldLogsList(byteArrayOldLogs, byteArrayLogs);
                }
            }
        }

        #region Read Functions
        private void ReadOldLogsList(byte[] byteArrayOldLogs, byte[] byteArrayLogs)
        {
            MemoryStream xls = new MemoryStream(byteArrayLogs); 
            Workbook workbook = new Workbook();
            workbook.LoadFromStream(xls);
            Worksheet sheet = workbook.Worksheets[0];
            int newRow = sheet.Rows.Length + 1;
            
            MemoryStream ms = new MemoryStream(byteArrayOldLogs);
            string data;
            string logId;
            string info;
            string tipo;
            string dataencomenda;
            string[] lineSplit;
            Regist regist;
            ExtraOrder order;
            using (var sr = new StreamReader(ms))
            {
                foreach(var line in sr.ReadToEnd().Split('\n'))
                {
                    try
                    {
                        if (line.Equals(""))
                            continue;

                        regist = null;
                        order = null;

                        lineSplit = line.Split(':');
                        tipo = lineSplit[0].Split(' ')[0].Trim();

                        if (tipo.Equals("LOCALIZAÇÃO"))
                            continue;

                        //get id
                        logId = lineSplit[0];
                        logId = logId.Substring(logId.IndexOf("[") + 1);
                        logId = logId.Substring(0, logId.IndexOf("]"));

                        //get data
                        data = line.Split('-')[1];

                        if (DateTime.Parse(data) < DateTime.Today.AddDays(-15 )/*.AddMonths(-1)*/)
                            continue;

                        if (tipo.Equals("PAGAMENTO"))
                        {
                            //get info
                            info = lineSplit[1];
                            info = info.Substring(1, info.IndexOf("€") + 1);

                            regist = new Regist() { 
                                DetailRegistDay = DateTime.Parse(data),
                                Info = info,
                                ClientId = int.Parse(logId),
                                DetailType = DetailTypeEnum.Payment
                            }; 
                        }
                        else if (tipo.Equals("EXTRA"))
                        {
                            //get info
                            info = lineSplit[1];
                            info = info.Substring(1, info.IndexOf("€") + 1);

                            regist = new Regist()
                            {
                                DetailRegistDay = DateTime.Parse(data),
                                Info = info,
                                ClientId = int.Parse(logId),
                                DetailType = DetailTypeEnum.AddExtra
                            };
                        }
                        else if (tipo.Equals("EDICAO"))
                        {
                            info = lineSplit[1].Replace(",", "\n");
                            info = info.Substring(1, info.IndexOf("-"));

                            regist = new Regist()
                            {
                                DetailRegistDay = DateTime.Parse(data),
                                Info = info,
                                ClientId = int.Parse(logId),
                                DetailType = DetailTypeEnum.Edit
                            };
                        }
                        else if (tipo.Equals("SOBRA"))
                        {
                            info = lineSplit[1].Replace(",", "\n");
                            info = info.Substring(1, info.IndexOf("-"));
                            //TODO: global regist
                            regist = null;
                        }
                        else if (tipo.Equals("ENCOMENDA"))
                        {
                            info = lineSplit[1].Replace(",", "\n");
                            info = info.Replace("&", ".");
                            info = info.Substring(1, info.IndexOf("-"));

                            dataencomenda = lineSplit[1].Split(' ')[4].Split(',')[0];

                            order = new ExtraOrder() {
                                ClientId = int.Parse(logId),
                                OrderRegistDay = DateTime.Parse(data),
                                OrderDay = DateTime.Parse(dataencomenda),
                                AllItems = GetListItemsFromOldRegist(info),
                                IsTotal = true,
                                StoreOrder = false
                            };
                        }
                        else if (tipo.Equals("NOVOCLIENTE"))
                        {
                            info = lineSplit[1];
                            info = info.Substring(1, info.IndexOf("-"));

                            regist = new Regist()
                            {
                                DetailRegistDay = DateTime.Parse(data),
                                Info = info,
                                ClientId = int.Parse(logId),
                                DetailType = DetailTypeEnum.NewClient
                            };
                        }
                        else if (tipo.Equals("REGISTO"))
                        {
                            info = lineSplit[1].Replace(",", "\n");
                            info = info.Replace("&", ".");
                            info = info.Substring(1, info.IndexOf("-"));

                            dataencomenda = lineSplit[1].Split(' ')[4].Split(',')[0];

                            order = new ExtraOrder() {
                                ClientId = int.Parse(logId),
                                OrderRegistDay = DateTime.Parse(data),
                                OrderDay = DateTime.Parse(dataencomenda),
                                AllItems = GetListItemsFromOldRegist(info),
                                IsTotal = true,
                                StoreOrder = false
                            };
                        }


                        if (regist != null)
                        {
                            _clientsManagerService.SetNewRegist(regist.ClientId, regist);

                            sheet.InsertRow(newRow);

                            sheet.Range[newRow, (int)LogsListItemsPositions.ID].Text = regist.ClientId.ToString();
                            sheet.Range[newRow, (int)LogsListItemsPositions.Type].Text = regist.DetailType.ToString();
                            sheet.Range[newRow, (int)LogsListItemsPositions.Info].Text = regist.Info;
                            sheet.Range[newRow, (int)LogsListItemsPositions.Date].Text = regist.DetailRegistDay.ToString("dd/MM/yyyy");

                            newRow += 1;
                        }
                        if(order != null)
                        {
                            _clientsManagerService.SetNewOrder(order.ClientId, order);

                            sheet.InsertRow(newRow);

                            sheet.Range[newRow, (int)LogsListItemsPositions.ID].Text = order.ClientId.ToString();
                            sheet.Range[newRow, (int)LogsListItemsPositions.Type].Text = order.DetailType.ToString();
                            sheet.Range[newRow, (int)LogsListItemsPositions.Info].Text = string.Empty;
                            sheet.Range[newRow, (int)LogsListItemsPositions.Date].Text = order.OrderRegistDay.ToString("dd/MM/yyyy");
                            sheet.Range[newRow, (int)LogsListItemsPositions.OrderDay].Text = order.OrderDay.ToString("dd/MM/yyyy");
                            sheet.Range[newRow, (int)LogsListItemsPositions.Order].Text = GetOrderStringDb(order.AllItems);
                            sheet.Range[newRow, (int)LogsListItemsPositions.IsAll].Text = order.IsTotal ? "1" : "0";

                            newRow += 1;
                        }

                    } catch(Exception e)
                    {
                        Debug.Write("----------------------------Error in line: " + line);
                    }
                }

                /*
                 * String line;

            while ((line = br.readLine()) != null) {


                else if (tipo.equals("REGISTO")) {
                    info = lineSplit[1].replaceAll(",", "\n");
                    info = info.replaceAll("&", ".");
                    info = info.substring(1, info.indexOf("-"));

                    info = info.replaceAll("]", "\t");
                    info = info.replaceAll(";", "-");

                    registo = new RegistoLoja(Integer.parseInt(logId), formatter.parse(data), info, new ExtraEncomenda(Integer.parseInt(logId)));

                }

                else if (tipo.equals("TOTALENCOMENDA")) {
                    info = lineSplit[1].replaceAll(",", "\n");
                    info = info.replaceAll("&", ".");
                    info = info.substring(1, info.indexOf("-"));
                    if (tipo.equals("REGISTO")) {
                        info = info.replaceAll("]", "\t");
                        info = info.replaceAll(";", "-");
                    }
                    //TODO: global regist

                }

                else if (tipo.equals("INATIVIDADE")) {
                    info = lineSplit[1];
                    info = info.substring(1, info.indexOf("-"));

                    registo = new RegistoInatividade(Integer.parseInt(logId), formatter.parse(data), info);


                }
                else if (tipo.equals("REMOVECLIENTE")) {
                    info = lineSplit[1];
                    info = info.substring(1, info.indexOf("-"));

                    registo = new RegistoDeleteCliente(Integer.parseInt(logId), formatter.parse(data), info);
                }

                else if (tipo.equals("INFO")) {
                    //get info
                    info = lineSplit[1];
                    info = info.substring(1, info.indexOf("-"));

                    registo = new RegistoGenerico(Integer.parseInt(logId), formatter.parse(data), info);

                }

                else {
                    continue;
                }
                 */
            }
            MemoryStream output = new MemoryStream();
            workbook.SaveToStream(output, FileFormat.Version97to2003);
            _fileService.SaveFile(LogsListFileName, output.ToArray(), true);
        }
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
            bool storeLabel;
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
                storeLabel = sheet.Range[i, (int)LogsListItemsPositions.StoreLabel].DisplayedText.Equals("1");

                if (detailType == DetailTypeEnum.Order)
                {
                    _clientsManagerService.SetNewOrder(
                        clientId,
                        new ExtraOrder()
                        {
                            ClientId = clientId,
                            OrderRegistDay = detailDate,
                            OrderDay = orderDate,
                            AllItems = GetOrderListItems(extraOrderDesc),
                            IsTotal = total,
                            StoreOrder = storeLabel
                        });
                }
                else
                {
                    _clientsManagerService.SetNewRegist(
                        clientId,
                        new Regist()
                        {
                            DetailRegistDay = detailDate,
                            Info = info,
                            ClientId = clientId,
                            DetailType = detailType
                        }
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
            List<ReSaleValues> reSaleValues = new List<ReSaleValues>();
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

                reSaleValues = new List<ReSaleValues>();
                for (int k = (int)ProductsListItemsPositions.PVP + 1; k < sheet.Columns.Length; k++)
                {
                    idConverted = int.TryParse(sheet.Range[1, k].DisplayedText, out clientId);
                    if (!idConverted) break;
                    productValue = double.Parse(sheet.Range[i, k].DisplayedText.Replace(".", ","));

                    reSaleValues.Add(new ReSaleValues() { ClientId = clientId, Value = productValue });
                }

                productsList.Add(new Product() { 
                    Name = name,
                    Id = id,
                    ImageReference = imageReference,
                    Unity = unity,
                    ProductType = productType,
                    PVP = pvp,
                    ReSaleValues = reSaleValues
                });
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
            string phoneNumber;
            Address address;
            DateTime paymentDate;
            string lastDateChangestr;
            DateTime lastDateChange;
            string type;
            PaymentTypeEnum paymentType;
            bool active;
            double extraValueToPay;
            DateTime? startStopDate;
            DateTime? endStopDate;
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
                
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].DisplayedText);
                door = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Door].DisplayedText);
                extraValueToPay = double.Parse(sheet.Range[i, (int)ClientsListItemsPositions.Extra].DisplayedText.Replace(".", ","));
                name = sheet.Range[i, (int)ClientsListItemsPositions.Name].DisplayedText;
                subName = sheet.Range[i, (int)ClientsListItemsPositions.SubName].DisplayedText;
                addressDesc = sheet.Range[i, (int)ClientsListItemsPositions.AddressDesc].DisplayedText;
                coord = sheet.Range[i, (int)ClientsListItemsPositions.Coordinates].DisplayedText;
                paymentDate = DateTime.ParseExact(sheet.Range[i, (int)ClientsListItemsPositions.Payment].DisplayedText, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (sheet.Range[i, (int)ClientsListItemsPositions.StartStop].DisplayedText.Equals("-"))
                    startStopDate = null;
                else
                    startStopDate = DateTime.ParseExact(sheet.Range[i, (int)ClientsListItemsPositions.StartStop].DisplayedText, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (sheet.Range[i, (int)ClientsListItemsPositions.EndStop].DisplayedText.Equals("-"))
                    endStopDate = null;
                else
                    endStopDate = DateTime.ParseExact(sheet.Range[i, (int)ClientsListItemsPositions.EndStop].DisplayedText, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                type = sheet.Range[i, (int)ClientsListItemsPositions.PaymentType].DisplayedText;
                active = sheet.Range[i, (int)ClientsListItemsPositions.Active].DisplayedText.Equals("1");
                phoneNumber = sheet.Range[i, (int)ClientsListItemsPositions.PhoneNumber].DisplayedText;
                phoneNumber = phoneNumber.Equals("") ? "Sem numero" : phoneNumber;
                lastDateChangestr = sheet.Range[i, (int)ClientsListItemsPositions.LastDateChange].DisplayedText;
                lastDateChange = lastDateChangestr.Equals("") ? DateTime.MinValue : DateTime.Parse(lastDateChangestr);


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

                //address = new Address(addressDesc, door, coord);
                paymentType = GetPaymentType(type);

                if(startStopDate != null && endStopDate != null)
                {
                    if (DateTime.Today >= startStopDate?.Date && DateTime.Today <= endStopDate?.Date)
                        active = false;
                    else if (DateTime.Today > endStopDate?.Date) {
                        startStopDate = null;
                        endStopDate = null;
                        active = true;
                        //TODO save changes (ao ler pela primeira vez depois de uma paragem o clinete volta a ativo)
                    }
                }
                //address = new Address(addressDesc, door, coord);

                Client client = new Client()
                {
                    Id = id,
                    Name = name,
                    SubName = subName,
                    Address = new Address()
                    {
                        AddressDesc = addressDesc,
                        NumberDoor = door,
                        Coordenadas = coord
                    },
                    PaymentDate = paymentDate,
                    StartDayStopService = startStopDate,
                    LastDayStopService = endStopDate,
                    PaymentType = paymentType,
                    Active = active,
                    ExtraValueToPay = extraValueToPay,
                    DailyOrders = dailyOrdersList,
                    PhoneNumber = phoneNumber,
                    LastChangeDate = lastDateChange,
                    DetailsList = new List<Regist>(),
                    ExtraOrdersList = new List<ExtraOrder>()
                };

                clientsList.Add(client);
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
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].DisplayedText);

                if(id == client.Id)
                {
                    sheet.Range[i, (int)ClientsListItemsPositions.Payment].Text = client.PaymentDate.ToString("dd/MM/yyyy");
                    sheet.Range[i, (int)ClientsListItemsPositions.StartStop].Text = client.StartDayStopService?.ToString("dd/MM/yyyy") ?? "-";
                    sheet.Range[i, (int)ClientsListItemsPositions.EndStop].Text = client.LastDayStopService?.ToString("dd/MM/yyyy") ?? "-"; 
                    sheet.Range[i, (int)ClientsListItemsPositions.Name].Text = client.Name;
                    sheet.Range[i, (int)ClientsListItemsPositions.SubName].Text = client.SubName;
                    sheet.Range[i, (int)ClientsListItemsPositions.AddressDesc].Text = client.Address.AddressDesc;
                    sheet.Range[i, (int)ClientsListItemsPositions.Door].Text = client.Address.NumberDoor.ToString();
                    sheet.Range[i, (int)ClientsListItemsPositions.Coordinates].Text = client.Address.Coordenadas;
                    sheet.Range[i, (int)ClientsListItemsPositions.PaymentType].Text = GetPaymentType(client.PaymentType);
                    sheet.Range[i, (int)ClientsListItemsPositions.Extra].Text = client.ExtraValueToPay.ToString();
                    sheet.Range[i, (int)ClientsListItemsPositions.Active].Text = client.Active ? "1" : "0";
                    sheet.Range[i, (int)ClientsListItemsPositions.PhoneNumber].Text = client.PhoneNumber;

                    client.LastChangeDate = DateTime.Today;
                    sheet.Range[i, (int)ClientsListItemsPositions.LastDateChange].Text = client.LastChangeDate.ToString("dd/MM/yyyy");

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
            sheet.Range[newRow, (int)LogsListItemsPositions.StoreLabel].Text = regist.StoreOrder ? "1" : "0";

            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }
        #endregion

        #region Delete
        public void RemoveClient(Client client)
        {
            byte[] byteArray = _fileService.GetFile(ClientsListFileName);
            byte[] byteArrayUpdated = DeleteClientDB(client, byteArray);
            _fileService.SaveFile(ClientsListFileName, byteArrayUpdated, true);
        }
        private byte[] DeleteClientDB(Client client, byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            MemoryStream output = new MemoryStream();

            Workbook workbook = new Workbook();
            workbook.LoadFromStream(ms);
            Worksheet sheet = workbook.Worksheets[0];

            int id;

            for (int i = 2; i <= sheet.Rows.Length; i++)
            {
                id = int.Parse(sheet.Range[i, (int)ClientsListItemsPositions.ID].DisplayedText);
                if(id == client.Id)
                {
                    sheet.DeleteRow(i);
                    break;
                }
            }
            workbook.SaveToStream(output, FileFormat.Version97to2003);
            return output.ToArray();
        }

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
            bool storeOrder;
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
                storeOrder = sheet.Range[i, (int)LogsListItemsPositions.StoreLabel].DisplayedText.Equals("1");

                if (detailType == DetailTypeEnum.Order)
                {

                    ExtraOrder order = new ExtraOrder()
                    {
                        ClientId = clientId,
                        OrderRegistDay = detailDate,
                        OrderDay = orderDate,
                        AllItems = GetOrderListItems(extraOrderDesc),
                        IsTotal = total,
                        StoreOrder = storeOrder
                    };
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
        private List<DailyOrderDetails> GetListItemsFromOldRegist(string quantidades)
        {
            List<DailyOrderDetails> items = new List<DailyOrderDetails>();

            int produtoId;
            double quantidade;
            bool first = true;
            string temp;

            foreach (string array in quantidades.Split('\n'))
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                if (array.Contains("Total"))
                    continue;

                temp = array.Substring(1);
                temp = Regex.Replace(temp, @"[^\u0000-\u007F]+", string.Empty);
                string[] allcomponents = Regex.Split(temp, ";");
                produtoId = _productsManagerService.GetProductByClosestName(allcomponents[0].Trim())?.Id ?? -1;
                if(produtoId != -1)
                {
                    quantidade = 0;
                    double.TryParse(array.Split(';')[1].Replace("\n", ""), out quantidade);
                    if (quantidade > 0)
                        items.Add(new DailyOrderDetails() { ProductId = produtoId, Ammount = quantidade});
                }
            }

            return items;
        }
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
            else if (DetailTypeEnum.Edit.ToString().Equals(displayedText))
            {
                return DetailTypeEnum.Edit;
            }
            else
            {
                return DetailTypeEnum.None;
            }
        }
        private List<DailyOrderDetails> GetOrderListItems(string displayedText)
        {
            List<DailyOrderDetails> allItems = new List<DailyOrderDetails>();

            int produtoId;
            double quantidade;

            foreach (string array in displayedText.Split(';'))
            {
                if (array.Equals("-") || array.Equals(""))
                    break;

                produtoId = int.Parse(array.Split('-')[0]);
                quantidade = double.Parse(array.Split('-')[1]);

                allItems.Add(new DailyOrderDetails() { ProductId = produtoId, Ammount = quantidade});
            }

            return allItems;
        }
        private DailyOrder GetDailyOrder(string orderDesc, DayOfWeek day)
        {
            List<DailyOrderDetails> allItems = new List<DailyOrderDetails>();

            int produtoId;
            double quantidade;

            foreach (string array in orderDesc.Split(';'))
            {
                if (array.Equals("-") || array.Equals(""))
                    break;

                produtoId = int.Parse(array.Split('-')[0]);
                quantidade = double.Parse(array.Split('-')[1]);

                if(produtoId != 148)
                    allItems.Add(new DailyOrderDetails { ProductId = produtoId, Ammount = quantidade});
            }

            return new DailyOrder() { DayOfWeek = day, AllItems = allItems };
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
        private string GetOrderStringDb(List<DailyOrderDetails> allItems)
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
