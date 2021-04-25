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

        public File SaveFile(string fileName, byte[] data, bool overwrite = false)
        {
            try
            {
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
    }
}