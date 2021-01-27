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
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class ClientOrderAdapter : RecyclerView.Adapter
    {
        private List<Product> orderProducts;

        public ClientOrderAdapter(List<Product> orderProducts)
        {
            this.orderProducts = orderProducts;
        }

        public override int ItemCount => orderProducts.Count + 1;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is EmptyListViewHolder)
            {
                var vh = holder as EmptyListViewHolder;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (ItemCount == 1)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EmptyListItems, parent, false);
                return new EmptyListViewHolder(view);

            } else
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EmptyListItems, parent, false);
                return new EmptyListViewHolder(view);
            }
        }
    }
}