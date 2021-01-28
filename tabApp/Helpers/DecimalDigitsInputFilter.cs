using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tabApp.Helpers
{
    public class DecimalDigitsInputFilter : Java.Lang.Object, IInputFilter
    {

        public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            int dotPos = -1;
            int len = dest.Length();
            for (int i = 0; i < len; i++)
            {
                char c = dest.CharAt(i);
                if (c == '.' || c == ',')
                {
                    dotPos = i;
                    break;
                }
            }
            if (dotPos >= 0)
            {

                // protects against many dots
                if (source.Equals(new Java.Lang.String(".")) || source.Equals(new Java.Lang.String(",")))
                {
                    return source.SubSequenceFormatted(0, source.Length() - 1);
                }
                // if the text is entered before the dot
                if (dend <= dotPos)
                {
                    return null;
                }
                if (len - dotPos > DecimalDigits)
                {
                    return source.SubSequenceFormatted(0, (source.Length() - 1) == - 1 ? 0 : source.Length() - 1);
                }
            }

            return null;
        }

        private readonly int DecimalDigits;

        public DecimalDigitsInputFilter(int decimalDigits)
        {
            this.DecimalDigits = decimalDigits;
        }
    }
}