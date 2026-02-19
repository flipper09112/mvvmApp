using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Clients
{
    public class GetDataToPrintService : IGetDataToPrintService
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IAmmountToPayService _ammountToPayService;

        private const string Separator = "--------------------------------\n";
        private const string End = "\n\n\n\n";
        private const string HeaderText = "Artigos de Padaria e Pastelaria\nao Domicilio\n\nDistribuidor(a): Maria Manuela\nContacto: 964690528\n";

        public GetDataToPrintService(IChooseClientService chooseClientService, IOrdersManagerService ordersManagerService, IClientsManagerService clientsManagerService
                                     , IProductsManagerService productsManagerService, IAmmountToPayService ammountToPayService)
        {
            _chooseClientService = chooseClientService;
            _ordersManagerService = ordersManagerService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
            _ammountToPayService = ammountToPayService;
        }

        private string Hearder()
        {
            return HeaderText + "Data: " + DateTime.Now.ToString("dd/MM/yyyy           HH:mm") + "\n" + Separator;
        }

        private string ClientDetails()
        {
            string text = "Cliente : " + _chooseClientService.ClientSelected.Name + "\nNo. Cliente: " + _chooseClientService.ClientSelected.Id + "\n\n";

            text += "Despesa correspondente:\n";
            text += "De: " + _chooseClientService.ClientSelected.PaymentDate.AddDays(1).ToString("dd/MM/yyyy") + "  ate   " 
                + _chooseClientService.PayTo.ToString("dd/MM/yyyy") + "\n" + Separator;

            return text;
        }
        private string MainData()
        {
            string text = "QTD  Dias          Preco   Total\n";

            double segAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.SegDailyOrder);
            double terAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.TerDailyOrder);
            double quaAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.QuaDailyOrder);
            double quiAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.QuiDailyOrder);
            double sexAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.SexDailyOrder);
            double sabAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.SabDailyOrder);
            double domAmmount = _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, _chooseClientService.ClientSelected.DomDailyOrder);

            int segDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Monday);
            int terDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Tuesday);
            int quaDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Wednesday);
            int quiDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Thursday);
            int sexDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Friday);
            int sabDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Saturday);
            int domDays = CountDaysBetweenDates(_chooseClientService.ClientSelected.PaymentDate, _chooseClientService.PayTo, DayOfWeek.Sunday);

            string segunda = segDays + "," + "SEGUNDA" + "," + segAmmount.ToString("N2").Replace(",", ".") + "," + (segDays * segAmmount).ToString("N2").Replace(",", ".");
            string terca = terDays + "," + "TERCA" + "," + terAmmount.ToString("N2").Replace(",", ".") + "," + (terDays * terAmmount).ToString("N2").Replace(",", ".");
            string quarta = quaDays + "," + "QUARTA" + "," + quaAmmount.ToString("N2").Replace(",", ".") + "," + (quaDays * quiAmmount).ToString("N2").Replace(",", ".");
            string quinta = quiDays + "," + "QUINTA" + "," + quiAmmount.ToString("N2").Replace(",", ".") + "," + (quiDays * sexAmmount).ToString("N2").Replace(",", ".");
            string sexta = sexDays + "," + "SEXTA" + "," + sexAmmount.ToString("N2").Replace(",", ".") + "," + (sexDays * sexAmmount).ToString("N2").Replace(",", ".");
            string sabado = sexDays + "," + "SABADO" + "," + sabAmmount.ToString("N2").Replace(",", ".") + "," + (sabDays * sabAmmount).ToString("N2").Replace(",", ".");
            string domingo = domDays + "," + "DOMINGO" + "," + domAmmount.ToString("N2").Replace(",", ".") + "," + (domDays * domAmmount).ToString("N2").Replace(",", ".");
            string extras = 1 + "," + "EXTRAS" + "," + _chooseClientService.ClientSelected.ExtraValueToPay.ToString("N2").Replace(",", ".") + "," + _chooseClientService.ClientSelected.ExtraValueToPay.ToString("N2").Replace(",", ".");

            text += FixSpaces(segunda) + "\n";
            text += FixSpaces(terca) + "\n";
            text += FixSpaces(quarta) + "\n";
            text += FixSpaces(quinta) + "\n";
            text += FixSpaces(sexta) + "\n";
            text += FixSpaces(sabado) + "\n";
            text += FixSpaces(domingo) + "\n";
            text += FixSpaces(extras) + "\n";

            return text + Separator;
        }

        private string StoreData()
        {
            string text = "";
            DateTime dateTemp = _chooseClientService.ClientSelected.PaymentDate.AddDays(1);
            int count = 0;

            while (dateTemp.Date <= _chooseClientService.PayTo)
            {
                ExtraOrder extraOrder = _clientsManagerService.HasOrderThisDate(_chooseClientService.ClientSelected, dateTemp);

                if(extraOrder == null)
                {
                    text += dateTemp.ToString("dd/MM/yyyy") + "                  0.00\n";
                }
                else
                {
                    text += FixSpaces2Items(dateTemp.ToString("dd/MM/yyyy") + "," + _ordersManagerService.GetValue(_chooseClientService.ClientSelected.Id, extraOrder.AllItems).ToString("N2").Replace(",", ".") + "\n");

                    foreach (var item in extraOrder.AllItems)
                    {
                        text += _productsManagerService.GetProductById(item.ProductId).Name + " - " + item.Ammount.ToString("N2") + "\n";
                    }
                }

                dateTemp = dateTemp.AddDays(1);
            }
            return text + "\n" + Separator;
        }

        private string FixSpaces(string text)
        {
            string result = "";
            int[] spaces = { 5, 19, 26, 32 };
            int remain = 0;

            for (int i = 0; i < text.Split(',').Length; i++)
            {
                if (i == 3)
                {
                    remain = spaces[i] - text.Split(',')[i].Length - result.Length;
                    while (remain > 0)
                    {
                        result += " ";
                        remain--;
                    }
                }
                result += text.Split(',')[i];
                remain = spaces[i] - result.Length;
                while (remain > 0)
                {
                    result += " ";
                    remain--;
                }
            }
            return result;
        }
        private string FixSpaces2Items(string text)
        {
            string result = "";
            int[] spaces = { 10, 32 };
            int remain = 0;

            for (int i = 0; i < text.Split(',').Length; i++)
            {
                if (i == 1)
                {
                    remain = spaces[i] - text.Split(',')[i].Length - result.Length;
                    while (remain > 0)
                    {
                        result += " ";
                        remain--;
                    }
                }
                result += text.Split(',')[i];
                remain = spaces[i] - result.Length;
                while (remain > 0)
                {
                    result += " ";
                    remain--;
                }
            }
            return result;
        }

        private int CountDaysBetweenDates(DateTime start, DateTime end, DayOfWeek dayOfWeek)
        {
            DateTime dateTemp = start.AddDays(1);
            int count = 0;

            while(dateTemp.Date <= end.Date)
            {
                if (dayOfWeek == dateTemp.DayOfWeek)
                    count++;

                dateTemp = dateTemp.AddDays(1);
            }

            return count;
        }

        private string EndPrint()
        {
            string total = "Total";
            string valor = _ammountToPayService.Calculate(_chooseClientService.ClientSelected, _chooseClientService.PayTo).ToString("N2");

            int remain = 32 - 5 - valor.Length;
            for (int i = 0; i < remain; i++)
            {
                total += " ";
            }

            total += valor;

            return total + "\n\nObrigado pela Preferencia" + End;
        }

        private string NormalPrint()
        {
            return Hearder() 
                + ClientDetails() 
                + MainData()
                + EndPrint();
        }
        private string StorePrint()
        {
            return Hearder()
                + ClientDetails()
                + StoreData()
                + EndPrint();
        }

        public List<PrintPreview> GetDataToPrint(Client clientSelected)
        {
            List<PrintPreview> item = new List<PrintPreview>();
            item.Add(new PrintPreview(NormalPrint(), "Normal"));
            item.Add(new PrintPreview(StorePrint(), "Loja"));
            return item;
        }
    }

}
