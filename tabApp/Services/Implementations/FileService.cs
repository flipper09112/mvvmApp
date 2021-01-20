using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Services
{
    public class FileService : IFileService
    {
        private string internalPath = (string) Android.App.Application.Context.FilesDir;

        public bool HasFile(string fileName)
        {
            File file = new File(internalPath, fileName);
            if (file.Exists())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public File GetFile(string fileName)
        {
            return new File(internalPath, fileName);
        }

        public void SaveFile(string fileName, byte[] data)
        {
            try
            {
                File f = new File(internalPath, fileName);
                if (!f.Exists())
                    f.CreateNewFile();
                FileOutputStream fos = new FileOutputStream(f);
                fos.Write(data);
                fos.Flush();
                fos.Close();
            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        }
    }
}