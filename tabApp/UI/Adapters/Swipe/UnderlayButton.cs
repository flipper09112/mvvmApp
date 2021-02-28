using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace tabApp.UI.Adapters.Swipe
{
    public class UnderlayButton
    {
        private string text;
        private int imageResId;
        private Color color;
        private int pos;
        private RectF clickRegion;
        private ICommand clickCommand;

        public UnderlayButton(String text, int imageResId, Color color, ICommand clickCommand)
        {
            this.text = text;
            this.imageResId = imageResId;
            this.color = color;
            this.clickCommand = clickCommand;
        }

        public bool OnClick(float x, float y)
        {
            if (clickRegion != null && clickRegion.Contains(x, y))
            {
                clickCommand?.Execute(pos);
                return true;
            }

            return false;
        }

        public void OnDraw(Canvas c, RectF rect, int pos, Context context)
        {
            Paint p = new Paint();

            // Draw background
            p.Color = color;
            c.DrawRect(rect, p);

            // Draw Text
            p.Color = Color.White;
            //p.setTextSize(LayoutHelper.getPx(MyApplication.getAppContext(), 12));
            p.TextSize = context.Resources.DisplayMetrics.Density * 25;

            Rect r = new Rect();
            float cHeight = rect.Height();
            float cWidth = rect.Width();
            p.TextAlign = Paint.Align.Left;
            p.GetTextBounds(text, 0, text.Length, r);

            float x = cWidth / 2f - r.Width() / 2f - r.Left;
            float y = cHeight / 2f + r.Height() / 2f - r.Bottom;
            c.DrawText(text, rect.Left + x, rect.Top + y, p);

            clickRegion = rect;
            this.pos = pos;
        }
    }
}