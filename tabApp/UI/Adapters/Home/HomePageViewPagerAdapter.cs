using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.UI.Adapters.ViewPager;

namespace tabApp.UI.Adapters.Home
{
    public class HomePageViewPagerAdapter : PagerAdapter
    {
        public List<SecondaryOptions> TabsOptions;
        private HomePageOrdersListAdapter ordersAdapter;

        public override int Count => TabsOptions?.Count ?? 0;
        public HomeViewModel ViewModel { get; internal set; }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view;

            if (TabsOptions[position].Name.Equals("Encomendas"))
            {
                view = GetTodayOrders(container, position);
            }
            else
            {
                view = GetOtherView(container, position);
            }

            container.AddView(view);
            return view;
        }


        private View GetTodayOrders(ViewGroup container, int position)
        {
            Android.Content.Context context = container.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            if (((OrdersPage)TabsOptions[position]).Value.Count == 0)
                return inflater.Inflate(Resource.Layout.EmptyListItems, container, false);

            ordersAdapter = new HomePageOrdersListAdapter((OrdersPage)TabsOptions[position], ViewModel);

            var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(context);
            recycler.SetLayoutManager(layoutManager);
            recycler.HasFixedSize = true;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                recycler.NestedScrollingEnabled = true;
            }
            recycler.SetAdapter(ordersAdapter);

            return view;
        }

        private View GetOtherView(ViewGroup container, int position)
        {
            Android.Content.Context context = container.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            /*var adapter = new AdministrativeAdapter(MovementHistoryViewModel)
            {
                ItemsSource = list,
                Administrative = Administratives,
                NoResultsText = NoResultsText
            };*/

            var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recycler.SetBackgroundResource(position == 1 ? Resource.Color.bg_JuntaDias : Resource.Color.bg_LojaSemana);
            /*var layoutManager = new LinearLayoutManager(context);
            recycler.SetLayoutManager(layoutManager);
            recycler.HasFixedSize = true;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                recycler.NestedScrollingEnabled = true;
            }
            recycler.SetAdapter(adapter);*/

            return view;
        }
        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override void NotifyDataSetChanged()
        {
            /*ordersAdapter.ExtraOrdersList = ((OrdersPage)ViewModel.TabsOptions[0]).Value;
            ordersAdapter.NotifyDataSetChanged();*/
            base.NotifyDataSetChanged();
        }
    }
}