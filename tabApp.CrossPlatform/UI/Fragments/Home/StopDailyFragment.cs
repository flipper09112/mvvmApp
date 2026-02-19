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
using tabApp.Core.ViewModels.Home;

namespace tabApp.UI.Fragments.Home
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class StopDailyFragment : BaseFragment<StopDailyViewModel>
    {
        private MainActivity _activity;
        private TextView _pageTitle;
        private Switch _indeterminatedSwitch;
        private Switch _determinatedSwitch;
        private EditText _firstDayDeterminatedEdt;
        private EditText _firstDayIndeterminatedEdt;
        private EditText _lastDayDeterminatedEdt;
        private Button _saveButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.StopDailyFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            _indeterminatedSwitch = view.FindViewById<Switch>(Resource.Id.indeterminatedSwitch);
            _determinatedSwitch = view.FindViewById<Switch>(Resource.Id.determinatedSwitch);
            _firstDayDeterminatedEdt = view.FindViewById<EditText>(Resource.Id.firstDayDeterminatedEdt);
            _firstDayIndeterminatedEdt = view.FindViewById<EditText>(Resource.Id.firstDayIndeterminatedEdt);
            _lastDayDeterminatedEdt = view.FindViewById<EditText>(Resource.Id.lastDayDeterminatedEdt);
            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.SaveCommand.CanExecuteChanged -= SaveCommandCanExecuteChanged;
            ViewModel.GoBack -= GoBack;
            _firstDayDeterminatedEdt.Click -= OpenCalendarEdt;
            _firstDayIndeterminatedEdt.Click -= OpenCalendarEdt;
            _lastDayDeterminatedEdt.Click -= OpenCalendarEdt;
            _saveButton.Click -= SaveButtonClick;
            _determinatedSwitch.Click -= DeterminatedSwitchClick;
            _indeterminatedSwitch.Click -= IndeterminatedSwitchClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            _pageTitle.Text = ViewModel.Client.Name;
            _firstDayDeterminatedEdt.Text = ViewModel.FirstDayDeterminatedDate.ToString("dd/MM/yyyy");
            _firstDayIndeterminatedEdt.Text = ViewModel.FirstDayIndeterminatedDate.ToString("dd/MM/yyyy");
            _lastDayDeterminatedEdt.Text = ViewModel.LastDayDeterminatedDate.ToString("dd/MM/yyyy");

            SaveCommandCanExecuteChanged(null, null);
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.SaveCommand.CanExecuteChanged += SaveCommandCanExecuteChanged;
            ViewModel.GoBack += GoBack;
            _firstDayDeterminatedEdt.Click += OpenCalendarEdt;
            _firstDayIndeterminatedEdt.Click += OpenCalendarEdt;
            _lastDayDeterminatedEdt.Click += OpenCalendarEdt;
            _saveButton.Click += SaveButtonClick;
            _determinatedSwitch.Click += DeterminatedSwitchClick;
            _indeterminatedSwitch.Click += IndeterminatedSwitchClick;
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void IndeterminatedSwitchClick(object sender, EventArgs e)
        {
            _determinatedSwitch.Checked = false;
            ViewModel.SelectTypeCommand.Execute(StopTypeEnum.InDeterminated);
        }

        private void DeterminatedSwitchClick(object sender, EventArgs e)
        {
            _indeterminatedSwitch.Checked = false;
            ViewModel.SelectTypeCommand.Execute(StopTypeEnum.Determinated);
        }

        private void SaveCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.SaveCommand.CanExecute(null))
            {
                _saveButton.Enabled = true;
            }
            else
            {
                _saveButton.Enabled = false;
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if(ViewModel.SaveCommand.CanExecute(null))
            {
                ViewModel.SaveCommand.Execute(null);
            }
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.FirstDayDeterminatedDate):
                    _firstDayDeterminatedEdt.Text = ViewModel.FirstDayDeterminatedDate.ToString("dd/MM/yyyy");
                    break;
                case nameof(ViewModel.FirstDayIndeterminatedDate):
                    _firstDayIndeterminatedEdt.Text = ViewModel.FirstDayIndeterminatedDate.ToString("dd/MM/yyyy");
                    break;
                case nameof(ViewModel.LastDayDeterminatedDate):
                    _lastDayDeterminatedEdt.Text = ViewModel.LastDayDeterminatedDate.ToString("dd/MM/yyyy");
                    break;
            }
        }

        private void OpenCalendarEdt(object sender, EventArgs e)
        {
            EditText editText = (EditText)sender;
            if(editText == _firstDayDeterminatedEdt)
            {
                ViewModel.ShowCalendarPickerCommand.Execute(DateTypeEnum.FirstDayDeterminated);
            }
            else if (editText == _firstDayIndeterminatedEdt)
            {
                ViewModel.ShowCalendarPickerCommand.Execute(DateTypeEnum.FirstDayIndeterminated);
            }
            else if (editText == _lastDayDeterminatedEdt)
            {
                ViewModel.ShowCalendarPickerCommand.Execute(DateTypeEnum.LastDayDeterminated);
            }
        }
    }
}