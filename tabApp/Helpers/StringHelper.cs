using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace tabApp.Helpers
{
    public static class StringHelper
    {
        public static SpannableString SetBold(string boldText, string normalText)
        {
            SpannableString str = new SpannableString(boldText + normalText);
            str.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, boldText.Length, SpanTypes.ExclusiveExclusive);
            return str;
        }
        public static bool ContainsInsensitive(this string source, string search)
        {
            return (new CultureInfo("pt-PT").CompareInfo).IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}