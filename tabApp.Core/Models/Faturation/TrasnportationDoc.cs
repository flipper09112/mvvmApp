using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.Faturation
{
    public class TrasnportationDoc
    {
        public string Name { get; internal set; }
        public string DocumentUrl { get; internal set; }
        public DateTime EmissionDate { get; internal set; }
        public int ID { get; internal set; }
    }
}
