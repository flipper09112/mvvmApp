using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Helpers;

namespace tabApp.UI.Adapters
{
    public class OtherOptionsGridAdapter : BaseAdapter
    {
        private List<Option> options;

        public OtherOptionsGridAdapter(List<Option> options)
        {
            this.options = options;
        }

        public override int Count => options?.Count ?? 0;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Content.Context context = parent.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.OtherOptionItem, parent, false);

            TextView name = view.FindViewById<TextView>(Resource.Id.android_gridview_text);
            CardView layout = view.FindViewById<CardView>(Resource.Id.android_custom_gridview_layout);
            ImageView image = view.FindViewById<ImageView>(Resource.Id.android_gridview_image);
            name.Text = options[position].Name;
            image.SetImageResource(ImageHelper.GetImageResource(context, options[position].ImageName));

            layout.Click += delegate {
                options[position].Action?.Execute(null);
            };

            return view;
        }


    }
}