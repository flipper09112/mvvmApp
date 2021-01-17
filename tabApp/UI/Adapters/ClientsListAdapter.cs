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
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class ClientsListAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => 3;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ClientListItem, parent, false);
            return new ClientViewHolder(view);
        }
    }
}