using Firebase.Storage;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core.Services.Implementations
{
    public class FirebaseService : IFirebaseService
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("gestor-tab-2.appspot.com");
        public async Task<byte[]> GetUrlDownload(string nameFile, EventHandler updatePercentageDownloadEvent = null)
        {
            string url = await firebaseStorage
                .Child("docs")
                .Child(nameFile)
                .GetDownloadUrlAsync();

            //
            var processMsgHander = new ProgressMessageHandler(new HttpClientHandler());
            processMsgHander.HttpSendProgress += (sender, e) =>
            {
                //add your codes base on e.BytesTransferred and e.ProgressPercentage
            };

            processMsgHander.HttpReceiveProgress += (sender, e) =>
            {
                updatePercentageDownloadEvent?.Invoke(null, e);
                //add your codes base on e.BytesTransferred and e.ProgressPercentage
                Console.WriteLine("Download db (%): " + e.ProgressPercentage);
            };
            //
            using (var client = new HttpClient(processMsgHander))
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }

        public async Task SaveFile(string nameFile, byte[] fileBytes, EventHandler updatePercentageUploadEvent = null)
        {
            Stream stream = new MemoryStream(fileBytes);

            var task = firebaseStorage
                       .Child("docs")
                       .Child(nameFile)
                       .PutAsync(stream);

            task.Progress.ProgressChanged += (s, e) => updatePercentageUploadEvent?.Invoke(e.Percentage, null);

            await task;
        }
    }
}
