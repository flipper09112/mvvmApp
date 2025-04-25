using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using tabApp.Core.Services.Implementations.Faturation;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using static SQLite.SQLite3;

namespace tabApp.Core.Services.Interfaces.WebServices.Bases
{
    public abstract class BaseRequest<Tinput, Toutput> where Tinput : BaseInput where Toutput : BaseOutput
    {
        protected virtual HttpMethod HttpMethod { get; } = HttpMethod.Post;
        protected abstract string EndPoint { get; }
        protected virtual bool HasId { get; } = false;

        public async Task<Toutput> SendAsync(Tinput input)
        {
            try
            {
                string endPointEdited = EndPoint;
                if (HasId)
                {
                    endPointEdited = endPointEdited.Replace(":id", input.Id);
                }

                Debug.WriteLine("Request : " + FaturationService.BaseUrl + endPointEdited);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(FaturationService.BaseUrl + endPointEdited);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = HttpMethod.ToString().ToUpper();

                // 🔒 Adiciona o Bearer Token aqui:
                httpWebRequest.Headers["Authorization"] = "Bearer " + input.ApiToken;

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

                    // Tratamento especial para quando a resposta é só um número
                    if (typeof(Toutput) == typeof(DuplicateWayBillOutput) && long.TryParse(result, out _))
                    {
                        var output = new DuplicateWayBillOutput { id = result };
                        return (Toutput)(object)output;
                    } 

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
