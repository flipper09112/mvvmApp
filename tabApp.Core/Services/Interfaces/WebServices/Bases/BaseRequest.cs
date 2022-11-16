using Newtonsoft.Json;
using Spire.Pdf.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using tabApp.Core.Services.Implementations.Faturation;
using static SQLite.SQLite3;

namespace tabApp.Core.Services.Interfaces.WebServices.Bases
{
    public abstract class BaseRequest<Tinput, Toutput> where Tinput : BaseInput where Toutput : BaseOutput
    {
        protected abstract HttpMethod HttpMethod { get; }
        protected abstract string EndPoint { get; }

        public async Task<Toutput> SendAsync(Tinput input)
        {
            try
            {
                Debug.WriteLine("Request : " + FaturationService.BaseUrl + EndPoint);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(FaturationService.BaseUrl + EndPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = HttpMethod.ToString().ToUpper();

                Debug.WriteLine("Method : " + HttpMethod.ToString().ToUpper());

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var body = JsonConvert.SerializeObject(input);
                    Debug.WriteLine("Body : " + body);
                    streamWriter.Write(body);
                }

                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Debug.WriteLine("Response : " + result);
                    return JsonConvert.DeserializeObject<Toutput>(result);
                }
            }
            catch(Exception e)
            {
                var output = (Toutput)Activator.CreateInstance(typeof(Toutput));
                output.Error = e.Message;
                Debug.WriteLine("Response : " + e.Message);

                return output;
            }

            return null;
        }
    }
}
