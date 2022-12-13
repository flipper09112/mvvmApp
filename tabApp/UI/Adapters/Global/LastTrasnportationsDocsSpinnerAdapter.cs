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
using tabApp.Core.Models.Faturation;

namespace tabApp.UI.Adapters.Global
{
    internal class LastTrasnportationsDocsSpinnerAdapter : BaseAdapter<TrasnportationDoc>
    {
        private List<TrasnportationDoc> lastTrasnportationsDocs;

        public LastTrasnportationsDocsSpinnerAdapter(List<TrasnportationDoc> lastTrasnportationsDocs)
        {
            this.lastTrasnportationsDocs = lastTrasnportationsDocs;
        }

        public override TrasnportationDoc this[int position] => lastTrasnportationsDocs[position];

        public override int Count => lastTrasnportationsDocs?.Count ?? 0;

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
            dateLabel.Text = lastTrasnportationsDocs[position].Name + "(" + lastTrasnportationsDocs[position].EmissionDate.ToString("ddd dd/MM/yyyy") + ")";
            dateLabel.TextSize = 15f;

            return view;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent);
        }
    }
}