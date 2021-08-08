using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.Global
{
    public class HomeFinancialsProductsAdapter : RecyclerView.Adapter
    {
        private List<ProductAmmount> productsList;

        public HomeFinancialsProductsAdapter(List<ProductAmmount> productsList)
        {
            this.productsList = productsList;
        }

        public override int ItemCount => productsList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is HomeFinancialsProductsViewHolder)
            {
                var vh = holder as HomeFinancialsProductsViewHolder;
                vh.Bind(productsList[holder.AdapterPosition], true);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);
            var view = inflater.Inflate(Resource.Layout.ProductOrderItem, parent, false);
            return new HomeFinancialsProductsViewHolder(view);
        }
    }
}