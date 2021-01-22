using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Helpers;

namespace tabApp.Core.Services.Implementations.Helpers
{
    public class GetSpinnerDatesService : IGetSpinnerDatesService
    {
        private Client _client;

        public List<DateTime> GetListDatesToPay(Client client)
        {
            _client = client;

            List<DateTime> dateTimesList = new List<DateTime>();

            switch(client.PaymentType)
            {
                case PaymentTypeEnum.JuntaDias:
                case PaymentTypeEnum.Diario:
                    DatesDailyPayment(dateTimesList);
                    break;
                case PaymentTypeEnum.Loja:
                case PaymentTypeEnum.Semanal:
                    DatesWeeklyPayment(dateTimesList);
                    break;
                case PaymentTypeEnum.Mensal:
                    DatesMensalPayment(dateTimesList);
                    break;
                default:
                    return dateTimesList;
            }

            return dateTimesList;
        }

        private void DatesMensalPayment(List<DateTime> dateTimesList)
        {
            DateTime dateTemp = _client.PaymentDate;

            while ((dateTemp - DateTime.Today).TotalDays < 0)
            {
                dateTemp = dateTemp.AddMonths(1);
                dateTemp = dateTemp.AddDays(-(dateTemp.Day - 1));

                dateTemp = dateTemp.AddDays(-1);
                dateTimesList.Add(dateTemp);
                dateTemp = dateTemp.AddDays(1);
            }

            dateTimesList.Reverse();

            //condição de quando a data de pagamento está á frente do dia de hj
            if (dateTimesList.Count == 0)
            {
                dateTemp = _client.PaymentDate;
                dateTemp = dateTemp.AddMonths(1);
                dateTemp = dateTemp.AddDays(-(dateTemp.Day - 1));
                dateTemp = dateTemp.AddDays(-1);

                dateTimesList.Add(dateTemp);
                return;
            }

            if (DateTime.Today.Day < 15 && dateTimesList.Count > 1)
            {
                DateTime toMove = dateTimesList[1];
                dateTimesList[1] = dateTimesList[0];
                dateTimesList[0] = toMove;
            }
        }

        private void DatesWeeklyPayment(List<DateTime> dateTimesList)
        {
            DateTime dateTemp = _client.PaymentDate;
            dateTemp.AddDays(1);

            bool antesQuarta = false;
            if (DateTime.Today.DayOfWeek <= DayOfWeek.Wednesday && !(DateTime.Today.DayOfWeek == DayOfWeek.Sunday))
            {
                antesQuarta = true;
            }

            if (_client.ExtraValueToPay > 0)
            {
                if (dateTemp.DayOfWeek == DayOfWeek.Sunday)
                {
                    dateTimesList.Add(dateTemp);
                }
            }

            while ((dateTemp - DateTime.Today).TotalDays != 0)
            {
                if (dateTemp.DayOfWeek == DayOfWeek.Sunday)
                {
                    dateTimesList.Add(dateTemp);
                }
                dateTemp = dateTemp.AddDays(1);
            }

            if (!dateTimesList.Contains(DateTime.Today.NextSunday()))
                dateTimesList.Add(DateTime.Today.NextSunday());

            dateTimesList.Reverse();

            if (antesQuarta && dateTimesList.Count > 1)
            {
                DateTime toMove = dateTimesList[1];
                dateTimesList[1] = dateTimesList[0];
                dateTimesList[0] = toMove;
            }

            //condição de quando a data de pagamento está á frente do dia de hj
            if (dateTimesList.Count == 0)
            {
                dateTimesList.Add(DateTime.Today);
            }
        }

        private void DatesDailyPayment(List<DateTime> dateTimesList)
        {
            dateTimesList.Add(DateTime.Today);
        }
        

    }
}
