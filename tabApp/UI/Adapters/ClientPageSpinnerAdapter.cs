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

namespace tabApp.UI.Adapters
{
    public class ClientPageSpinnerAdapter : BaseAdapter<DateTime>
    {
        public override int Count => spinnerDates?.Count ?? 0;

        public override DateTime this[int position] => spinnerDates[position];

        private List<DateTime> spinnerDates;

        public ClientPageSpinnerAdapter(List<DateTime> spinnerDates)
        {
            this.spinnerDates = spinnerDates;
        }

        public override long GetItemId(int position)
        {
            return spinnerDates[position].Ticks;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Content.Context context = parent.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.SpinnerDateItem, parent, false);

            TextView dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
            dateLabel.Text = spinnerDates[position].ToString("dd/MM/yyyy");

            return view;
        }
    }
}