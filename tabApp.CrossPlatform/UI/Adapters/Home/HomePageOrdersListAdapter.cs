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
using tabApp.Core;
using tabApp.Core.Models;
using tabApp.Core.ViewModels.Global;
using tabApp.Core.ViewModels.Snooze;
using tabApp.UI.Fragments.Snooze;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.Home
{
    public class HomePageOrdersListAdapter : RecyclerView.Adapter
    {
        public List<(Client Client, ExtraOrder ExtraOrder)> ExtraOrdersList;
        private SnoozeViewModel viewModel2;
        private HomeViewModel viewModel;
        private OrdersPage ordersPage;
        private List<(Client Client, ExtraOrder ExtraOrder)> tomorrowOrders;
        private GlobalOrderViewModel viewModel1;
        private List<(Client Client, ExtraOrder ExtraOrder)> todayOrders;

        public HomePageOrdersListAdapter(OrdersPage ordersPage, HomeViewModel viewModel)
        {
            this.ordersPage = ordersPage;
            ExtraOrdersList = ordersPage.Value;
            this.viewModel = viewModel;
        }

        public HomePageOrdersListAdapter(List<(Client Client, ExtraOrder ExtraOrder)> tomorrowOrders, GlobalOrderViewModel viewModel1)
        {
            ExtraOrdersList = tomorrowOrders;
            this.viewModel1 = viewModel1;
        }

        public HomePageOrdersListAdapter(List<(Client Client, ExtraOrder ExtraOrder)> todayOrders, SnoozeViewModel viewModel2)
        {
            ExtraOrdersList = todayOrders;
            this.viewModel2 = viewModel2;
        }

        public override int ItemCount => ExtraOrdersList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            HomePageOrderViewHolder vh = holder as HomePageOrderViewHolder;
            if(viewModel != null)
                vh.Bind(ExtraOrdersList[holder.AdapterPosition], viewModel);
            else if(viewModel1 != null)
                vh.Bind(ExtraOrdersList[holder.AdapterPosition], viewModel1);
            else if (viewModel2 != null)
                vh.Bind(ExtraOrdersList[holder.AdapterPosition], viewModel2);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomePagerOrderItem, parent, false);
            return new HomePageOrderViewHolder(view);
        }
    }
}