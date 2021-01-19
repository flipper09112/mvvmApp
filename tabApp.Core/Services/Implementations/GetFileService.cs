using Firebase.Storage;
using System.Net.Http;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core.Services.Implementations
{
    public class GetFileService : IGetFileService
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("gestor-tab-2.appspot.com");
        public async void GetUrlDownload(string nameFile)
        {
            string url = await firebaseStorage
                .Child("docs")
                .Child(nameFile)
                .GetDownloadUrlAsync();

            using (var client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                var byteArray = await response.Content.ReadAsByteArrayAsync();

                //Create an instance of ExcelEngine.
                /*using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel97to2003;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //Adding text to a cell
                    worksheet.Range["A1"].Text = "Hello World";

                    //Save the workbook to stream in xlsx format. 
                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    workbook.Close();

                    //Save the stream as a file in the device and invoke it for viewing
                    //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("GettingStared.xlsx", "application/msexcel", stream);
                }*/
            }
        }
    }
}
