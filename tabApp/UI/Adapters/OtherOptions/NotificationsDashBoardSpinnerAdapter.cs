using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.UI.Fragments.Global.Other;

namespace tabApp.UI.Adapters.OtherOptions
{
    public class NotificationsDashBoardSpinnerAdapter : BaseAdapter<NotificationsType>
    {
        public override NotificationsType this[int position] => notificationsTypesList[position];

        private List<NotificationsType> notificationsTypesList;

        public NotificationsDashBoardSpinnerAdapter(List<NotificationsType> notificationsTypesList)
        {
            this.notificationsTypesList = notificationsTypesList;
        }

        public override int Count => notificationsTypesList?.Count ?? 0;

        public override long GetItemId(int position)
        {
            return notificationsTypesList[position].GetHashCode();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Content.Context context = parent.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.SpinnerDateItem, parent, false);

            TextView dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
            dateLabel.Text = notificationsTypesList[position].Name;

            return view;
        }
    }
}