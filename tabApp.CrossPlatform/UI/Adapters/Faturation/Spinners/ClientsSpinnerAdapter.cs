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

namespace tabApp.UI.Adapters.Faturation.Spinners
{
    public class ClientsSpinnerAdapter : BaseAdapter<FatClient>
    {
        public List<FatClient> ClientsList { get; }

        public ClientsSpinnerAdapter(List<FatClient> clientsList)
        {
            ClientsList = clientsList;
        }

        public override FatClient this[int position] => ClientsList[position];

        public override int Count => ClientsList?.Count ?? 0;

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
            dateLabel.Text = ClientsList[position].Name;
            dateLabel.TextSize = 18f;

            return view;
        }
        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return GetView(position, convertView, parent);
        }
    }
}