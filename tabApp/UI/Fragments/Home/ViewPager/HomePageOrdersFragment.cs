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
using tabApp.Core;
using tabApp.UI.Adapters.Home;

namespace tabApp.UI.Fragments.Home.ViewPager
{
    public class HomePageOrdersFragment : Android.Support.V4.App.Fragment
    {
        public OrdersPage OrdersPage;
        private HomeViewModel ViewModel;
        private HomePageOrdersListAdapter adapter;
        private View emptyLayout;
        public RecyclerView recycler;

        public HomePageOrdersFragment(Core.OrdersPage ordersPage, Core.HomeViewModel viewModel)
        {
            this.OrdersPage = ordersPage;
            this.ViewModel = viewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomePageOrdersFragment, container, false);

            emptyLayout = view.FindViewById(Resource.Id.emptyLayout);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.listView);

            adapter = new HomePageOrdersListAdapter(OrdersPage, ViewModel);
            var layoutManager = new LinearLayoutManager(Context);

            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            UpdateUI();

            return view;
        }

        private void UpdateUI()
        {
            if (OrdersPage.Value.Count == 0)
            {
                emptyLayout.Visibility = ViewStates.Visible;
                recycler.Visibility = ViewStates.Invisible;
            }
            else
            {
                emptyLayout.Visibility = ViewStates.Invisible;
                recycler.Visibility = ViewStates.Visible;
            }
        }

        internal void UpdateOrdersList(bool setNewAdapter = false)
        {
            if(setNewAdapter)
            {
                SetNewAdapter();
            }
            else
            {
                adapter?.NotifyDataSetChanged();
            }

            UpdateUI();
        }

        private void SetNewAdapter()
        {
            adapter = new HomePageOrdersListAdapter(OrdersPage, ViewModel); 
            recycler.SetAdapter(adapter);
        }
    }
}