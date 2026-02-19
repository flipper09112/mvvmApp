using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.Notifications;
using tabApp.Helpers;

namespace tabApp.UI.Adapters.OtherOptions.Notifications
{
    public class CreateNoficationGridAdapter : BaseAdapter
    {
        public override int Count => _options?.Count ?? 0;
        private List<NotificationType> _options;

        public CreateNoficationGridAdapter(List<NotificationType> options)
        {
            this._options = options;
        }

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
            name.Text = _options[position].Name;
            image.SetImageResource(ImageHelper.GetImageResource(context, _options[position].ImageName));

            layout.Click += delegate {
                _options[position].Action?.Execute(null);
            };

            return view;
        }
    }
}