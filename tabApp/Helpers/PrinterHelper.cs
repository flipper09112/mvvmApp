using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace tabApp.Helpers
{
    public static class PrinterHelper
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }

        private static int[,] GetPixelsSlow(Bitmap image)
        {
            Width = image.Width;
            Height = image.Height;
            int[,] result = new int[Height, Width];
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    result[row, col] = image.GetPixel(col, row);
                }
            }

            return result;
        }

        public static int[,] GetLogo(Resources resource, int imageResource)
        {
            var bi = BitmapFactory.DecodeResource(resource, imageResource);
            int[,] pixels = GetPixelsSlow(bi);

            return pixels;
        }
    }
}