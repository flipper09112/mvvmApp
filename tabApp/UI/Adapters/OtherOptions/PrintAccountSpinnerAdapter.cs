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

namespace tabApp.UI.Adapters.OtherOptions
{
    public class PrintAccountSpinnerAdapter : BaseAdapter<string>
    {
        private List<string> pairedDevices;

        public PrintAccountSpinnerAdapter(List<string> pairedDevices)
        {
            this.pairedDevices = pairedDevices;
        }

        public override int Count => pairedDevices?.Count ?? 0;

        public override string this[int position] => pairedDevices[position];

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Content.Context context = parent.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.SpinnerDateItem, parent, false);

            TextView dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
            dateLabel.Text = pairedDevices[position];

            return view;
        }
        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent);
        }
    }
}