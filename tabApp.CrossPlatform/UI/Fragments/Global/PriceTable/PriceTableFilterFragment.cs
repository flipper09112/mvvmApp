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
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.PriceTable
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PriceTableFilterFragment : BaseFragment<PriceTableFilterViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _filterRv;
        private Button _saveButton;
        private Button _cleanFilter;
        private PriceTableFilterAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PriceTableFilterFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _filterRv = view.FindViewById<RecyclerView>(Resource.Id.filterRv);
            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            _cleanFilter = view.FindViewById<Button>(Resource.Id.cleanFilter);

            _filterRv.SetLayoutManager(new LinearLayoutManager(Context));
            _adapter = new PriceTableFilterAdapter(ViewModel, ViewModel.PriceTableFilterItem);
            _filterRv.SetAdapter(_adapter);

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.GoBack -= GoBack;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _saveButton.Click -= SaveButtonClick;
            _cleanFilter.Click -= CleanFilterClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
        }

        public override void SetupBindings()
        {
            ViewModel.GoBack += GoBack;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _saveButton.Click += SaveButtonClick;
            _cleanFilter.Click += CleanFilterClick;
        }

        private void CleanFilterClick(object sender, EventArgs e)
        {
            ViewModel.CleanFilterCommand.Execute(null);
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.ClientSelected):
                    _adapter.NotifyDataSetChanged();
                    break;
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute(null);
        }
    }
}