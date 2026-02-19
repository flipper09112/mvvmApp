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
using tabApp.Core.ViewModels.EditClient;
using tabApp.UI.Adapters.ClientPage.CreateNotification;

namespace tabApp.UI.Fragments.EditClient
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class CreateNotificationsFragment : BaseFragment<CreateNotificationsViewModel>
    {
        private MainActivity _activity;
        private Button _saveButton;
        private Button _addButton;
        private RecyclerView _otherDaysRv;
        private RecyclerView _preDefinedRv;
        private CreateNotificationsPredefinedAdapter _predefinedAdapter;
        private CreateNotificationsOtherDaysAdapterAdapter _otherDaysAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CreateNotificationsFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            _addButton = view.FindViewById<Button>(Resource.Id.addButton); 
            _otherDaysRv = view.FindViewById<RecyclerView>(Resource.Id.otherDaysRv);
            _preDefinedRv = view.FindViewById<RecyclerView>(Resource.Id.preDefinedRv);

            _preDefinedRv.SetLayoutManager(new LinearLayoutManager(Context));
            _otherDaysRv.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            SetPredefinedDaysList();
            SetOtherDaysList();
            _saveButton.Enabled = ViewModel.SaveCommand.CanExecute();
        }

        private void SetOtherDaysList()
        {
            if (_otherDaysAdapter != null) return;

            _otherDaysAdapter = new CreateNotificationsOtherDaysAdapterAdapter(ViewModel.ExtraDays);
            _otherDaysRv.SetAdapter(_otherDaysAdapter);
        }

        private void SetPredefinedDaysList()
        {
            if (_predefinedAdapter != null) return;

            _predefinedAdapter = new CreateNotificationsPredefinedAdapter(ViewModel, ViewModel.PreDefinedAlertItems);
            _preDefinedRv.SetAdapter(_predefinedAdapter);
        }

        public override void CleanBindings()
        {
            ViewModel.SaveCommand.CanExecuteChanged -= SaveCommandCanExecuteChanged;
            _addButton.Click -= AddButtonClick;
            _saveButton.Click -= SaveButtonClick;
            ViewModel.GoBack2Times -= GoBack2Times;
        }

        public override void SetupBindings()
        {
            ViewModel.SaveCommand.CanExecuteChanged += SaveCommandCanExecuteChanged;
            _addButton.Click += AddButtonClick;
            _saveButton.Click += SaveButtonClick;
            ViewModel.GoBack2Times += GoBack2Times;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            ViewModel.AddDateCommand.Execute();
        }

        private void GoBack2Times(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if(ViewModel.SaveCommand.CanExecute())
            {
                ViewModel.SaveCommand.Execute();
            }
        }

        private void SaveCommandCanExecuteChanged(object sender, EventArgs e)
        {
            _predefinedAdapter.NotifyDataSetChanged();
            _saveButton.Enabled = ViewModel.SaveCommand.CanExecute();
            SetNewDay();
        }

        private void SetNewDay()
        {
            if(ViewModel.NewDayAdded)
            {
                _otherDaysAdapter.NotifyItemInserted(ViewModel.ExtraDays.Count - 1);
                ViewModel.NewDayAdded = false;
            }
        }
    }
}