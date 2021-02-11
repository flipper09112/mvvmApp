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

namespace tabApp.UI.Adapters.Global
{
    public class PriceTableAdapter : RecyclerView.Adapter
    {
        private List<Product> allProducts;

        public PriceTableAdapter(List<Product> allProducts)
        {
            this.allProducts = allProducts;
        }

        public override int ItemCount => allProducts?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PriceTableViewHolder vh = holder as PriceTableViewHolder;
            vh.Bind(allProducts[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PriceTableItem, parent, false);
            return new PriceTableViewHolder(view);
        }
    }
}