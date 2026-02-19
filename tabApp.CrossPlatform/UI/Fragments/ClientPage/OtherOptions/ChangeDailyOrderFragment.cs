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
using tabApp.Core.ViewModels.ClientPage.OtherOptions;
using tabApp.UI.Fragments.EditClient;

namespace tabApp.UI.Fragments.ClientPage.OtherOptions
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ChangeDailyOrderFragment : BaseFragment<ChangeDailyOrderViewModel>
    {
        private MainActivity _activity;
        private TextView _dateLabel;
        private Button _saveButton;
        private FrameLayout _fragmentSpace;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ChangeDailyOrderFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _dateLabel = view.FindViewById<TextView>(Resource.Id.dateLabel);
            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            _fragmentSpace = view.FindViewById<FrameLayout>(Resource.Id.fragmentSpace);

            if(ChildFragmentManager.Fragments.Count == 0)
                ChildFragmentManager.BeginTransaction().Add(Resource.Id.fragmentSpace, new EditDailyOrdersFragment(ViewModel)).Commit();

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.SaveChangesCommand.CanExecuteChanged -= SaveChangesCommandCanExecuteChanged;
            _dateLabel.Click -= DateLabelClick;
            ViewModel.GoBack2Times -= GoBack2Times;
            _saveButton.Click -= SaveButtonClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            _dateLabel.Text = ViewModel.DateTime.ToString("dd/MM/yyyy");
            SaveChangesCommandCanExecuteChanged(null, null);
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.SaveChangesCommand.CanExecuteChanged += SaveChangesCommandCanExecuteChanged;
            _dateLabel.Click += DateLabelClick;
            ViewModel.GoBack2Times += GoBack2Times;
            _saveButton.Click += SaveButtonClick;
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            ViewModel.SaveChangesCommand.Execute(null);
        }

        private void GoBack2Times(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void SaveChangesCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.SaveChangesCommand.CanExecute(null))
                _saveButton.Enabled = true;
            else
                _saveButton.Enabled = false;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.DateTime):
                    _dateLabel.Text = ViewModel.DateTime.ToString("dd/MM/yyyy");
                    break;
            }
        }

        private void DateLabelClick(object sender, EventArgs e)
        {
            ViewModel.DatePickerDialogCommand.Execute(null);
        }
    }
}