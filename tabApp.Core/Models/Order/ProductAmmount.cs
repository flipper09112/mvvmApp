using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class ProductAmmount
    {
        public Product Product { get; set; }
        public double Ammount { get; set; }
    }

    [Table("DailyOrderDetails")]
    public class DailyOrderDetails
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DailyOrder))]
        public int DailyOrderId { get; set; }

        [ForeignKey(typeof(ExtraOrder))]
        public int ExtraOrderId { get; set; }

        public int ProductId { get; set; }

        public double Ammount { get; set; }
    }
    
}
