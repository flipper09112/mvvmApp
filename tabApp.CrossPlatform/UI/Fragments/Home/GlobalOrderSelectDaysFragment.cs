using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global;

namespace tabApp.UI.Fragments.Home
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class GlobalOrderSelectDaysFragment : BaseFragment<GlobalOrderSelectDaysViewModel>
    {
        private MainActivity _activity;
        private Button _backButton;
        private Button _confirmButton;
        private TextView _firstDayLabel;
        private TextView _lastDayLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.GlobalOrderSelectDaysFragment, container, false);

            _backButton = view.FindViewById<Button>(Resource.Id.backButton);
            _confirmButton = view.FindViewById<Button>(Resource.Id.confirmButton);
            _firstDayLabel = view.FindViewById<TextView>(Resource.Id.firstDayLabel);
            _lastDayLabel = view.FindViewById<TextView>(Resource.Id.lastDayLabel);

            _activity = ParentActivity as MainActivity;

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.GoBack -= GoBack;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.ConfirmCommand.CanExecuteChanged -= ConfirmCommandCanExecuteChanged;
            _backButton.Click -= BackButtonClick;
            _firstDayLabel.Click -= FirstDayLabelClick;
            _lastDayLabel.Click -= LastDayLabelClick;
            _confirmButton.Click -= ConfirmButtonClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            _firstDayLabel.Text = ViewModel.FirstDay.ToString("dd/MM/yyyy");
            _lastDayLabel.Text = ViewModel.LastDay.ToString("dd/MM/yyyy");
        }

        public override void SetupBindings()
        {
            ViewModel.GoBack += GoBack;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.ConfirmCommand.CanExecuteChanged += ConfirmCommandCanExecuteChanged;
            _backButton.Click += BackButtonClick;
            _firstDayLabel.Click += FirstDayLabelClick;
            _lastDayLabel.Click += LastDayLabelClick;
            _confirmButton.Click += ConfirmButtonClick;
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ConfirmCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if(ViewModel.ConfirmCommand.CanExecute(null))
            {
                _confirmButton.Enabled = true;
            } else
            {
                _confirmButton.Enabled = false;
            }
        }

        private void ConfirmButtonClick(object sender, EventArgs e)
        {
            ViewModel.ConfirmCommand.Execute(null);
        }

        private void LastDayLabelClick(object sender, EventArgs e)
        {
            ViewModel.SelectDateCommand.Execute(DayType.Last);
        }

        private void FirstDayLabelClick(object sender, EventArgs e)
        {
            ViewModel.SelectDateCommand.Execute(DayType.First);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }
    }
}