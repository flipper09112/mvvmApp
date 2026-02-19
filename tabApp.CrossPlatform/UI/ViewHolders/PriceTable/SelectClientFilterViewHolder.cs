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
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.ViewHolders.PriceTable
{
    public class SelectClientFilterViewHolder : RecyclerView.ViewHolder
    {
        private PriceTableFilterViewModel _viewModel;
        private RecyclerView _clientsRadiosRv;
        private SelectClientRadiosAdapter _adapter;

        public SelectClientFilterViewHolder(View itemView, PriceTableFilterViewModel _viewModel) : base(itemView)
        {
            this._viewModel = _viewModel; 

            _clientsRadiosRv = itemView.FindViewById<RecyclerView>(Resource.Id.clientsRadiosRv);
            _clientsRadiosRv.SetLayoutManager(new LinearLayoutManager(itemView.Context));
        }

        internal void Bind(PriceTableFilterItemModel priceTableFilterItemModel)
        {
            _adapter = new SelectClientRadiosAdapter(priceTableFilterItemModel, _viewModel);
            _clientsRadiosRv.SetAdapter(_adapter);
        }
    }
}