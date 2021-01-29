using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.ViewPager
{
    public class ClientPageOrdersListAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => _extraOrdersList?.Count ?? 0;
        private List<ExtraOrder> _extraOrdersList;
        private ClientPageViewModel viewModel;

        public ClientPageOrdersListAdapter(List<ExtraOrder> extraOrdersList, Core.ViewModels.ClientPageViewModel viewModel)
        {
            _extraOrdersList = extraOrdersList;
            this.viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrderViewHolder orderVH = holder as OrderViewHolder;
            orderVH.Bind(_extraOrdersList[holder.AdapterPosition], viewModel);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.OrderItem, parent, false);
            return new OrderViewHolder(view);
        }
    }
}