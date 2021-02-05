using Firebase.Storage;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core.Services.Implementations
{
    public class FirebaseService : IFirebaseService
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("gestor-tab-2.appspot.com");
        public async Task<byte[]> GetUrlDownload(string nameFile)
        {
            string url = await firebaseStorage
                .Child("docs")
                .Child(nameFile)
                .GetDownloadUrlAsync();

            using (var client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }

        public async Task SaveFile(string nameFile, byte[] fileBytes)
        {
            Stream stream = new MemoryStream(fileBytes);

            var task = await firebaseStorage
                       .Child("docs")
                       .Child(nameFile)
                       .PutAsync(stream);

        }
    }
}
