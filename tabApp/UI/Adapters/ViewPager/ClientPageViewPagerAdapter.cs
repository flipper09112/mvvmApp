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
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters.ViewPager;

namespace tabApp.UI.Adapters
{
    public class ClientPageViewPagerAdapter : PagerAdapter
    {
        private List<TabsOptionsEnum> tabsOptions;
        private Client client;

        public override int Count => tabsOptions?.Count ?? 0;

        public List<string> Title { get; internal set; }

        public ClientPageViewPagerAdapter(List<TabsOptionsEnum> tabsOptions, Core.Models.Client client)
        {
            this.tabsOptions = tabsOptions;
            this.client = client;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view;

            if (tabsOptions[position] == TabsOptionsEnum.Mapa)
            {
                view = GetMapView(container, position);
            }
            else if (tabsOptions[position] == TabsOptionsEnum.Registo)
            {
                view = GetDetailsView(container, position);
            }
            else if (tabsOptions[position] == TabsOptionsEnum.Encomendas)
            {
                view = GetOrdersView(container, position);
            }
            else
            {
                view = GetOtherView(container, position);
            }

            container.AddView(view);
            return view;
        }

        private View GetOrdersView(ViewGroup container, int position)
        {
            Android.Content.Context context = container.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            if (client.ExtraOrdersList.Count == 0)
                return inflater.Inflate(Resource.Layout.EmptyListItems, container, false);

            var adapter = new ClientPageOrdersListAdapter(client.ExtraOrdersList);
            var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(context);
            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            return view;
        }

        private View GetDetailsView(ViewGroup container, int position)
        {
            Android.Content.Context context = container.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            if(client.DetailsList.Count == 0)
                return inflater.Inflate(Resource.Layout.EmptyListItems, container, false);

            var adapter = new ClientPageDetailsAdapter(client.DetailsList);
            var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(context);
            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            return view;
        }

        private View GetMapView(ViewGroup container, int position)
        {
            Android.Content.Context context = container.Context;
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.CalendarLayout, container, false);

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
        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            Java.Lang.Object object1 = @object;
            container.RemoveView((View)object1);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override int GetItemPosition(Java.Lang.Object @object)
        {
            return PagerAdapter.PositionNone;
        }
    }
}