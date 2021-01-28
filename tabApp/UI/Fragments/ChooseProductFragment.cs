using Android.App;
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
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ChooseProductFragment : BaseFragment<ChooseProductViewModel>
    {
        private MainActivity _activity;
        private EditText _searchBar;
        private RecyclerView _productsRecyclerView;
        private ProductsItemsListAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ChooseProductFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _searchBar = view.FindViewById<EditText>(Resource.Id.editMobileNo);
            _productsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.productsRecyclerView);

            _productsRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            SetRecyclerView();
        }

        private void SetRecyclerView()
        {
            /*if(ViewModel.SearchWord.Length == 0)
            {*/
                _adapter = new ProductsItemsListAdapter(ViewModel.AllProducts, ViewModel.SearchWord, ViewModel.SelectProductCommand);
                _productsRecyclerView.SetAdapter(_adapter);
            /*} else
            {
                _adapter = new ProductsByTypeAdapter(ViewModel.AllProducts, ViewModel.SearchWord);
                _productsRecyclerView.SetAdapter(_adapter);
            }*/
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _searchBar.TextChanged -= SearchBarTextChanged;
        }
        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _searchBar.TextChanged += SearchBarTextChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void SearchBarTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ViewModel.SearchWord = _searchBar.Text;
        }
    }
}