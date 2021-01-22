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
using tabApp.Core.ViewModels;

namespace tabApp.UI.Adapters
{
    public class ClientPageViewPagerAdapter : PagerAdapter
    {
        private List<TabsOptionsEnum> tabsOptions;
        public override int Count => tabsOptions?.Count ?? 0;

        public List<string> Title { get; internal set; }

        public ClientPageViewPagerAdapter(List<TabsOptionsEnum> tabsOptions)
        {
            this.tabsOptions = tabsOptions;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view;

            if(position == 0)
            {
                view = GetMapView(container, position);
            } else
            {
                view = GetOtherView(container, position);
            }
            

            container.AddView(view);
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
            recycler.SetBackgroundResource(position == 0 ? Resource.Color.bg_JuntaDias : Resource.Color.bg_LojaSemana);
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

        public override int GetItemPosition(Java.Lang.Object @object)
        {
            return PagerAdapter.PositionNone;
        }
    }
}