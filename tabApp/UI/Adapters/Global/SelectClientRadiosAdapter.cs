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
    public class SelectClientRadiosAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => priceTableFilterItemModel.ClientsList?.Count ?? 0;

        private PriceTableFilterViewModel _viewModel;
        private PriceTableFilterItemModel priceTableFilterItemModel;

        public SelectClientRadiosAdapter(PriceTableFilterItemModel priceTableFilterItemModel, PriceTableFilterViewModel _viewModel)
        {
            this._viewModel = _viewModel;
            this.priceTableFilterItemModel = priceTableFilterItemModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is ClientRadioViewHolder clientRadioViewHolder)
            {
                clientRadioViewHolder.Bind(priceTableFilterItemModel, position, _viewModel.ClientSelected);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SelectClientRadio, parent, false);
            return new ClientRadioViewHolder(view);
        }
    }
}