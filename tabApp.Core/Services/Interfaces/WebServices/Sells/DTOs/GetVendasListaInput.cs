using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class GetVendasListaInput : BaseInput
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("search_in")]
        public string SearchIn { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SellsTypes Type { get; set; }

        [JsonProperty("skip")]
        public int Skip { get; set; }
    }

    public enum SellsTypes
    {
        Facturação,
        Orçamentos,
        Encomendas,
        Guias
    }
}
