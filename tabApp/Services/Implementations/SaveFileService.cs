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
    public class SaveFileService : ISaveFileService
    {
        public bool HasFile(string fileName)
        {
            File file = new File(Android.App.Application.Context.GetExternalFilesDir(null), fileName);
            if (file.Exists())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveFile(string fileName, byte[] data)
        {
        }
    }
}