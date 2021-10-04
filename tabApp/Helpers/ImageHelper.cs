using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tabApp.Helpers
{
    public static class ImageHelper
    {
        public static int GetImageResource(Context context, string imageName)
        {
            int x = context.Resources.GetIdentifier(imageName, "drawable", context.PackageName);
            return x;
        }
    }
}