using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Faturation;
using tabApp.UI.Adapters.Faturation.Spinners;
using tabApp.UI.Adapters.Faturation;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.UI.Fragments.Global.Faturation
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class FaturationFragment : BaseFragment<FaturationViewModel>
    {
        private RecyclerView _faturasRv;
        private RecyclerView _productsListRv;
        private View _emptyLayout;
        private ProductsListFatAdapter _productsListAdapter;
        private LastTrasnportationsDocsAdapter _lastTrasnportationsDocsAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FaturationFragment, container, false);

            _faturasRv = view.FindViewById<RecyclerView>(Resource.Id.faturasRv);
            _productsListRv = view.FindViewById<RecyclerView>(Resource.Id.productsListRv);
            _emptyLayout = view.FindViewById(Resource.Id.emptyLayout);

            return view;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.LastTrasnportationsDocs):
                    SetLastTrasnportationsDocsRv();
                    break;
                case nameof(ViewModel.ProductsList):
                    _productsListAdapter = new ProductsListFatAdapter(ViewModel.ProductsList);
                    _productsListAdapter.RemoveProduct = RemoveProduct;
                    _productsListRv.SetAdapter(_productsListAdapter);
                    break;
            }
        }

        private void RemoveProduct(FatItem fatItemToRemove)
        {
            int pos = ViewModel.ProductsList.IndexOf(fatItemToRemove);

            ViewModel.ProductsList.Remove(fatItemToRemove);
            _productsListAdapter.ProductsList = ViewModel.ProductsList;

            _productsListAdapter.NotifyItemRemoved(pos);
        }
        private void SetLastTrasnportationsDocsRv()
        {
            _emptyLayout.Visibility = ViewModel.LastTrasnportationsDocs == null || ViewModel.LastTrasnportationsDocs.Count == 0 ? ViewStates.Visible : ViewStates.Invisible;

            _lastTrasnportationsDocsAdapter = new LastTrasnportationsDocsAdapter(ViewModel.LastTrasnportationsDocs);
            _lastTrasnportationsDocsAdapter.DocClick = ViewModel.OpenDocCommand;
            _faturasRv.SetAdapter(_lastTrasnportationsDocsAdapter);
        }
    }
}