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
using tabApp.Core.ViewModels.Global.Other.Finance;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.Other.Finance
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class WeekFinancialsFragment : BaseFragment<WeekFinancialsViewModel>
    {
        private MainActivity _activity;
        private TextView _totalValue;
        private TextView _firstDateLabel;
        private TextView _lastDateLabel;
        private RecyclerView _daysList;
        private WeekFinancialsAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;

            View view = inflater.Inflate(Resource.Layout.WeekFinancialsFragment, container, false);

            _totalValue = view.FindViewById<TextView>(Resource.Id.totalValue);
            _firstDateLabel = view.FindViewById<TextView>(Resource.Id.firstDateLabel);
            _lastDateLabel = view.FindViewById<TextView>(Resource.Id.lastDateLabel);
            _daysList = view.FindViewById<RecyclerView>(Resource.Id.daysList);

            _daysList.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _firstDateLabel.Text = ViewModel.FirstDateSelected.ToString("dd/MM/yyyy");
            _lastDateLabel.Text = ViewModel.LastDateSelected.ToString("dd/MM/yyyy");
            _totalValue.Text = ViewModel.GetTotal();
            SetList();
        }

        private void SetList()
        {
            if (_adapter != null) return;

            _adapter = new WeekFinancialsAdapter(ViewModel.DaysAmmountList);
            _daysList.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _firstDateLabel.Click += FirstDateLabelClick;
            _lastDateLabel.Click += LastDateLabelClick;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _firstDateLabel.Click -= FirstDateLabelClick;
            _lastDateLabel.Click -= LastDateLabelClick;
            _adapter = null;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();

            if (e.PropertyName == nameof(ViewModel.DaysAmmountList)) {
                _adapter = null;
                SetList();
            } 
        }

        private void LastDateLabelClick(object sender, EventArgs e)
        {
            ViewModel.DateClickCommand.Execute(DateClickType.LastDate);
        }

        private void FirstDateLabelClick(object sender, EventArgs e)
        {
            ViewModel.DateClickCommand.Execute(DateClickType.FirstDate);
        }
    }
}