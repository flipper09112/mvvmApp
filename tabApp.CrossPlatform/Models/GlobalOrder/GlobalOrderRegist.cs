using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.GlobalOrder
{
    [Table("GlobalOrderRegist")]
    public class GlobalOrderRegist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime OrderRegistDate { get; set; }

        public string JsonData { get; set; }

        [Ignore]
        public List<ProductAmmount> ItemsList => JsonConvert.DeserializeObject<List<ProductAmmount>>(JsonData);
    }
}
