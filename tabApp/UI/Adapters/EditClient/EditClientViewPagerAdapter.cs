using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.UI.Fragments.EditClient;

namespace tabApp.UI.Adapters.EditClient
{
    public class EditClientViewPagerAdapter : FragmentStatePagerAdapter
    {
        public EditClientViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
        }

        /*private List<EditFieldsEnum> tabsOptions;
private Client client;
private EditClientViewModel viewModel;

public EditClientViewPagerAdapter(List<EditFieldsEnum> tabsOptions, Client client, EditClientViewModel viewModel)
{
   this.tabsOptions = tabsOptions;
   this.client = client;
   this.viewModel = viewModel;
}

public override int Count => tabsOptions?.Count ?? 0;

public List<string> Title { get; internal set; }

public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
{
   View view = null;
   if (position == 0)
   {
       view = GetProfileView(container, position);

   }else if(position == 1)
   {
       view = GetMapView(container, position);
   }
   else
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
   var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

   return view;
}

private View GetProfileView(ViewGroup container, int position)
{
   Android.Content.Context context = container.Context;
   var inflater = LayoutInflater.From(context);
   var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

   var adapter = new EditClientProfileItemsAdapter(viewModel.ProfileItems);

   var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
   var layoutManager = new LinearLayoutManager(context);
   recycler.SetLayoutManager(layoutManager);
   recycler.SetAdapter(adapter);

   return view;
}

private View GetOtherView(ViewGroup container, int position)
{
   Android.Content.Context context = container.Context;
   var inflater = LayoutInflater.From(context);
   var view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);


   var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
   recycler.SetBackgroundResource(position == 1 ? Resource.Color.bg_JuntaDias : Resource.Color.bg_LojaSemana);


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
}*/
        //public override int Count => Title?.Count ?? 0;

        public List<string> Title { get; internal set; }
        public EditClientViewModel ViewModel { get; internal set; }

        public override int Count => 3;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if(position == 0)
            {
                return new EditClientProfileFragment(ViewModel, ViewModel.ProfileItems);

            } else if (position == 1)
            {
                return new EditClientMapFragment(ViewModel);
            }
            else 
            {
                return new EditClientProfileFragment(ViewModel, ViewModel.AdminItems);
            }
            return null;
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {
            base.DestroyItem(container, position, @object);
        }
    }
}