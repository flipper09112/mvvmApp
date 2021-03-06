﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PriceTableFragment : BaseFragment<PriceTableViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _recyclerView;
        private EditText _searchEdt;
        private TextView _resultsLabel;
        private PriceTableAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PriceTableFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _searchEdt = view.FindViewById<EditText>(Resource.Id.editMobileNo);
            _resultsLabel = view.FindViewById<TextView>(Resource.Id.resultsLabel);

            _adapter = new PriceTableAdapter(ViewModel.AllProducts);
            _recyclerView.SetLayoutManager(new GridLayoutManager(Context, 3, GridLayoutManager.Vertical, false));
            _recyclerView.SetAdapter(_adapter);

            return view;
        }

        public override void CleanBindings()
        {
            _activity.ShowToolbar();
            _activity.ShowMenu();
            _searchEdt.TextChanged -= SearchEdtClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        public override void SetUI()
        {
            _adapter.AllProducts = ViewModel.AllProducts;
            _resultsLabel.Text = ViewModel.AllProducts.Count + " resultados encontrados";
            _adapter.NotifyDataSetChanged();
        }

        public override void SetupBindings()
        {
            _activity.HideToolbar();
            _activity.HideMenu();
            _searchEdt.TextChanged += SearchEdtClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void SearchEdtClick(object sender, EventArgs e)
        {
            ViewModel.SearchProduct = _searchEdt.Text;
        }
    }
}