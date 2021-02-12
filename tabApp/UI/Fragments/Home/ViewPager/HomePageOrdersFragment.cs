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
        private OrdersPage ordersPage;
        private HomeViewModel ViewModel;
        private HomePageOrdersListAdapter adapter;
        private RecyclerView recycler;

        public HomePageOrdersFragment(Core.OrdersPage ordersPage, Core.HomeViewModel viewModel)
        {
            this.ordersPage = ordersPage;
            this.ViewModel = viewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (ordersPage.Value.Count == 0)
                return inflater.Inflate(Resource.Layout.EmptyListItems, container, false);

            View view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            adapter = new HomePageOrdersListAdapter(ordersPage, ViewModel);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(Context);

            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            return view;
        }
    }
}