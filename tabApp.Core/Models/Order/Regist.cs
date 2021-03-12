using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace tabApp.Core.Models
{
    [Table("Regist")]
    [Serializable]
    public class Regist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DetailRegistDay { get; set; }
        public string Info { get; set; }

        [ForeignKey(typeof(Client))]
        public int ClientId { get; set; }
        public DetailTypeEnum DetailType { get; set; }

        /*public Regist(DateTime detailRegistDay, string info, int clientId, DetailTypeEnum detailType)
        {
            DetailRegistDay = detailRegistDay;
            Info = info;
            ClientId = clientId;
            DetailType = detailType;
        }*/
    }

    public enum DetailTypeEnum
    {
        Payment,
        Order,
        None,
        AddExtra,
        CancelOrder,
        Edit,
        NewClient,
        ChangeDailyOrder,
        Inativate
    }
}
