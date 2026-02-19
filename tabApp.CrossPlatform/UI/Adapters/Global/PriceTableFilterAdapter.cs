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
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.ViewHolders.PriceTable;

namespace tabApp.UI.Adapters.Global
{
    public class PriceTableFilterAdapter : RecyclerView.Adapter
    {
        private PriceTableFilterViewModel _viewModel;
        private List<PriceTableFilterItemModel> priceTableFilterItem;
        public override int ItemCount => priceTableFilterItem?.Count ?? 0;

        public PriceTableFilterAdapter(PriceTableFilterViewModel viewModel, List<PriceTableFilterItemModel> priceTableFilterItem)
        {
            _viewModel = viewModel;
            this.priceTableFilterItem = priceTableFilterItem;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is SelectClientFilterViewHolder selectClientFilterViewHolder)
            {
                selectClientFilterViewHolder.Bind(priceTableFilterItem[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SelectClientFilter, parent, false);
            return new SelectClientFilterViewHolder(view, _viewModel);
        }
    }
}