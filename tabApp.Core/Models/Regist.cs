using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class Regist
    {
        public DateTime DetailRegistDay { get; }
        public string Info { get; }
        public int ClientId { get; }
        public DetailTypeEnum DetailType { get; }

        public Regist(DateTime detailRegistDay, string info, int clientId, DetailTypeEnum detailType)
        {
            DetailRegistDay = detailRegistDay;
            Info = info;
            ClientId = clientId;
            DetailType = detailType;
        }
    }

    public enum DetailTypeEnum
    {
        Payment,
        Order,
        None,
        AddExtra
    }
}
