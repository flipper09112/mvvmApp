using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.Faturation
{
    public class FatClient
    {
        public string Name { get; internal set; }
        public string NIF { get; internal set; }
        public string Address { get; internal set; }
        public string Country { get; internal set; }
        public string PostalCode { get; internal set; }
        public int Id { get; internal set; }
        public string City { get; internal set; }
    }
}
