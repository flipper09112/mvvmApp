using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.Notifications
{
    [Table("Notification")]
    public class Notification
    {

        [PrimaryKey, AutoIncrement]
        public int NotificationId { get; set; }

        [ForeignKey(typeof(Client))]
        public int ClientId { get; set; }

        public string Info { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public DateTime AlertDay { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }

    public enum NotificationTypeEnum
    {
        None,
        DontPay,
        OrderChanged
    }
}
