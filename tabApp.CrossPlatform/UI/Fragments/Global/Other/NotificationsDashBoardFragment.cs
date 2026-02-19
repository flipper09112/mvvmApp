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
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.Global.Other
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class NotificationsDashBoardFragment : BaseFragment<NotificationsDashBoardViewModel>
    {
        private MainActivity _activity;
        private Spinner _notificationsTypeSpinner;
        private EditText _notificationsDate;
        private RecyclerView _notificationsList;
        private NotificationsDashBoardSpinnerAdapter _spinnerAdapter;
        private NotificationsDashBoardAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;

            View view = inflater.Inflate(Resource.Layout.NotificationsDashBoardFragment, container, false);

            _notificationsTypeSpinner = view.FindViewById<Spinner>(Resource.Id.notificationsTypeSpinner);
            _notificationsDate = view.FindViewById<EditText>(Resource.Id.notificationsDate);
            _notificationsList = view.FindViewById<RecyclerView>(Resource.Id.notificationsList);

            _notificationsList.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _adapter = new NotificationsDashBoardAdapter(ViewModel.NotificationListItems, ViewModel);
            _notificationsList.SetAdapter(_adapter);

            _spinnerAdapter = new NotificationsDashBoardSpinnerAdapter(ViewModel.NotificationsTypesList);
            _notificationsTypeSpinner.Adapter = _spinnerAdapter;
            _notificationsTypeSpinner.SetSelection(ViewModel.NotificationsTypesList.IndexOf(ViewModel.NotificationsTypeSelected)); 

            UpdateDate();
        }

        private void UpdateDate()
        {
            _notificationsDate.Focusable = false;
            _notificationsDate.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _notificationsDate.Click += NotificationsDateClick;
            _notificationsTypeSpinner.ItemSelected += NotificationsTypeSpinnerItemSelected;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _notificationsDate.Click -= NotificationsDateClick;
            _notificationsTypeSpinner.ItemSelected -= NotificationsTypeSpinnerItemSelected;
        }

        private void NotificationsTypeSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.SelectFilterCommand.Execute(_spinnerAdapter[e.Position]);
        }

        private void NotificationsDateClick(object sender, EventArgs e)
        {
            ViewModel.DateSelectCommand.Execute();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.AddOrderExtra):
                    _adapter.NotifyDataSetChanged();
                    break;

                case nameof(ViewModel.DateSelected):
                    UpdateDate();
                    break;

                case nameof(ViewModel.NotificationListItems):
                    SetUI();
                    break;
            }
        }
    }
}