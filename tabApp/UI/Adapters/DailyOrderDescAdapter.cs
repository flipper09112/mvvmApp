using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class DailyOrderDescAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => dailyItemsList?.Count ?? 0;

        private List<(string ProductName, string Ammount)> dailyItemsList;

        public DailyOrderDescAdapter(List<(string ProductName, string Ammount)> segDailyItemsList)
        {
            this.dailyItemsList = segDailyItemsList;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DailyOrderProductViewHolder productVH = holder as DailyOrderProductViewHolder;
            productVH.Bind(dailyItemsList[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DailyOrderProductItem, parent, false);
            return new DailyOrderProductViewHolder(view);
        }
    }
}