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
using tabApp.Core.ViewModels.Global.Other.Finance;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.Global
{
    public class WeekFinancialsAdapter : RecyclerView.Adapter
    {
        private List<DayAmmount> daysAmmountList;

        public WeekFinancialsAdapter(List<DayAmmount> daysAmmountList)
        {
            this.daysAmmountList = daysAmmountList;
        }

        public override int ItemCount => daysAmmountList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is WeekFinancialsItemViewHolder weekFinancialsItemVH) 
            {
                weekFinancialsItemVH.Bind(daysAmmountList[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.WeekFinancialsItem, parent, false);
            return new WeekFinancialsItemViewHolder(view);
        }
    }
}