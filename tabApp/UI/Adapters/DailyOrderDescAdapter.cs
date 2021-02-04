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
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class DailyOrderDescAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => dailyItemsList?.Count ?? segDailyItemsList?.Count ?? 0;

        private List<(Product Product, string Ammount)> dailyItemsList;
        private bool editVersion;
        private EditClientViewModel viewModel;
        private List<ClientProfileField> segDailyItemsList;

        public DailyOrderDescAdapter(List<(Product Product, string Ammount)> segDailyItemsList, bool editVersion = false)
        {
            this.dailyItemsList = segDailyItemsList;
            this.editVersion = editVersion;
        }

        public DailyOrderDescAdapter(List<ClientProfileField> segDailyItemsList, bool editVersion = true)
        {
            this.segDailyItemsList = segDailyItemsList;
            this.editVersion = editVersion;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DailyOrderProductViewHolder productVH = holder as DailyOrderProductViewHolder; 
            if (editVersion)
                productVH.Bind(segDailyItemsList[holder.AdapterPosition]);
            else
                productVH.Bind(dailyItemsList[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View view;
            if(editVersion)
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DailyOrderProductEditItem, parent, false);
            else
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DailyOrderProductItem, parent, false);

            return new DailyOrderProductViewHolder(view);
        }
    }
}