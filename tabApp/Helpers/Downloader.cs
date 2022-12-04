using Android.Gms.Tasks;
using Android.OS;
using Java.IO;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Helpers
{
    public static class Downloader
    {
        public static async Task<bool> DownloadFile(String fileURL, File directory)
        {
            try
            {
                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().PermitAll().Build();
                StrictMode.SetThreadPolicy(policy);

                FileOutputStream f = new FileOutputStream(directory);
                URL u = new URL(fileURL);
                HttpURLConnection c = (HttpURLConnection)u.OpenConnection();
                c.RequestMethod = "GET";
                c.DoOutput = true;
                await c.ConnectAsync();

                var inputStream = c.InputStream;

                byte[] buffer = new byte[1024];
                int len1 = 0;
                while ((len1 = inputStream.Read(buffer)) > 0) {
                    f.Write(buffer, 0, len1);
                }
                f.Close();

                return true;
            }
            catch (Exception e)
            {
                int c = 0;
            }

            return false;
        }
    }
}