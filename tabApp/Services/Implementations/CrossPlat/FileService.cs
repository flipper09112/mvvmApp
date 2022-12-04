using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.IO;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces;
using tabApp.Helpers;

namespace tabApp.Services
{
    public class FileService : IFileService
    {
        private File externalFileParent = Application.Context.GetExternalFilesDir(null);

        public FileService()
        {
            if (!externalFileParent.Exists())
                externalFileParent.Mkdir();
        }

        public bool HasFile(string fileName)
        {
            File file = new File(externalFileParent, fileName);
            if (file.Exists())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] GetFile(string fileName)
        {
            File f = new File(externalFileParent, fileName);

            return System.IO.File.ReadAllBytes(f.Path);
        }

        public async Task<bool> GetWebFile(string url, string fileName)
        {
            File folder = new File(externalFileParent, "pdfs");
            folder.Mkdir();
            File file = new File(folder, fileName);
            try
            {
                file.CreateNewFile();
            }
            catch (IOException e1)
            {
                e1.PrintStackTrace();
                return false;
            }

            await Downloader.DownloadFile(url, file);

            return true;
        }

        public object SaveFile(string fileName, byte[] data, bool overwrite = false)
        {
            try
            {
                if(fileName == DataBaseManagerService.DataBaseName)
                {

                }

                File f = new File(externalFileParent, fileName);
                if (!f.Exists())
                    f.CreateNewFile();
                else if (f.Exists() && overwrite)
                {
                    f.Delete();
                    f.CreateNewFile();
                }

                FileOutputStream fos = new FileOutputStream(f);
                fos.Write(data);
                fos.Flush();
                fos.Close();

                return f;
            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
            return null;
        }

        public void DeleteFile(string dataBaseName)
        {
            File file = new File(externalFileParent, dataBaseName);
            if (file.Exists())
            {
                file.Delete();
            }
        }

        public async void ShowPdfExternalApp(TrasnportationDoc documentSelected)
        {
            var hasFile = HasFile("/pdfs/" + documentSelected.Name);

            if(!hasFile)
            {
                await GetWebFile(documentSelected.DocumentUrl, documentSelected.Name);
            }

            ShowPdfExternalAppAndroid(documentSelected);
        }

        private void ShowPdfExternalAppAndroid(TrasnportationDoc documentSelected)
        {
            File file = new File(externalFileParent + "/pdfs/" + documentSelected.Name);
            PackageManager packageManager = CrossCurrentActivity.Current.AppContext.PackageManager;
            Intent testIntent = new Intent(Intent.ActionView);
            testIntent.SetType("application/pdf");
            var list = packageManager.QueryIntentActivities(testIntent, PackageInfoFlags.MatchDefaultOnly);
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionView);

            var uri = FileProvider.GetUriForFile(CrossCurrentActivity.Current.AppContext,
                                                 CrossCurrentActivity.Current.AppContext.PackageName + ".provider",
                                                 file); 
            intent.SetDataAndType(uri, "application/pdf");
            intent.SetFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            CrossCurrentActivity.Current.AppContext.StartActivity(intent);
        }

        public void ClearAllFilesFromPath()
        {
            File dir = new File(externalFileParent + "/pdfs");
            if (dir.IsDirectory)
            {
                String[] children = dir.List();
                for (int i = 0; i < children.Length; i++)
                {
                    new File(dir, children[i]).Delete();
                }
            }
        }
    }
}