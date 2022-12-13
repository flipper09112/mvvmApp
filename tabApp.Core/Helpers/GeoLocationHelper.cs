using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace tabApp.Core.Helpers
{
    public static class GeoLocationHelper
    {
        /*public static RootObject GetAddress(double lat, double lon)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + lat + "&lon=" + lon);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            return rootObject;
        }*/

        public static async Task<Placemark> GetAddress(double lat, double lon)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

                    Console.WriteLine(geocodeAddress);
                }

                return placemark;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }

            return null;
        }

        [DataContract]
        public class Address
        {
            [DataMember]
            public string road { get; set; }
            [DataMember]
            public string suburb { get; set; }
            [DataMember]
            public string city { get; set; }
            [DataMember]
            public string state_district { get; set; }
            [DataMember]
            public string state { get; set; }
            [DataMember]
            public string postcode { get; set; }
            [DataMember]
            public string country { get; set; }
            [DataMember]
            public string country_code { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string place_id { get; set; }
            [DataMember]
            public string licence { get; set; }
            [DataMember]
            public string osm_type { get; set; }
            [DataMember]
            public string osm_id { get; set; }
            [DataMember]
            public string lat { get; set; }
            [DataMember]
            public string lon { get; set; }
            [DataMember]
            public string display_name { get; set; }
            [DataMember]
            public Address address { get; set; }
        }
    }
}
