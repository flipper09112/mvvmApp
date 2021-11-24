using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace tabApp.Core.Helpers
{
    public static class SecureStorageHelper
    {
        public static string DatabaseDateDownloadKey = "DatabaseDateDownloadKey";
        public static string HasLoginKey = "LoginKey";
        public static string HasLoginYesValue = "HasLoginTrue";

        public static async Task SaveKeyAsync(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        public static async Task<string> GetKeyAsync(string key)
        {
            try
            {
                var oauthToken = await SecureStorage.GetAsync(key);
                return oauthToken;
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }

            return null;
        }
    }
}
