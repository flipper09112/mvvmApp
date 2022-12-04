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
        Inativate,
        ChangePrices,
        UpdateNIF
    }
}
