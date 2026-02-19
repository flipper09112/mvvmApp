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
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.ViewModels.Global;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class CakesTotalOrderAdapter : RecyclerView.Adapter
    {
        private List<CakeClientItem> cakesClients;
        private GlobalOrderViewModel viewModel;

        public CakesTotalOrderAdapter(List<CakeClientItem> cakesClients, GlobalOrderViewModel viewModel)
        {
            this.cakesClients = cakesClients;
            this.viewModel = viewModel;
        }

        public override int ItemCount => cakesClients?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            IndividualCakeOrderItemViewHolder vh = holder as IndividualCakeOrderItemViewHolder;
            vh.Bind(cakesClients[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.IndividualCakeOrderItem, parent, false);
            return new IndividualCakeOrderItemViewHolder(view);
        }
    }
}