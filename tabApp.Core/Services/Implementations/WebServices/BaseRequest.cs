using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces.WebServices;
using Xamarin.Essentials;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public abstract class BaseRequest<Tinput, Toutput> where Tinput : BaseInput where Toutput : BaseOutput
    {
        //private string BaseUrl = "http://94.61.92.97:92";
        private string BaseUrl = "http://94.61.92.97:95";

        protected abstract string ApiMethod { get; }

        public async Task<Toutput> Send(Tinput input, HttpMethod httpMethod)
        {
            if(!HasInternetConnection())
            {
                var result = GetObject<Toutput>();
                result.Success = false;
                result.ErrorMessage = "Sem conecção à internet. Ligue-se e tente novamente.";
                return result;
            }

            Uri uri = null;
            if (httpMethod == HttpMethod.Get)
            {
                uri = new Uri(BaseUrl + ApiMethod + GetInputParams(input));
            }

            Debug.WriteLine("request: " + uri.ToString());

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(1);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //Debug.WriteLine("answer: " + content);
                var item = JsonSerializer.Deserialize<Toutput>(content);

                item.Success = true;
                return item;
            }
            else
            {
                var result = GetObject<Toutput>();
                result.Success = false;
                result.ErrorMessage = "Serviço indisponivel. Por favor tente mais tarde!";
                return result;
            }

            return null;
        }

        private string GetInputParams(Tinput input)
        {
            return "/" + input.Id;
        }

        private bool HasInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        protected T GetObject<T>()
        {
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            return obj;
        }
    }
}