using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Faturation;
using tabApp.UI.Adapters;
using tabApp.UI.Adapters.Faturation;
using tabApp.UI.Adapters.Faturation.Spinners;
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.Global.Faturation
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class TransportationDocumentsFragment : BaseFragment<TransportationDocumentsViewModel>
    {
        private View _view;
        private RecyclerView _guiasRv;
        private RecyclerView _productsListRv;
        private View _emptyLayout;
        private Spinner _vehicleSpinner;
        private TextView _chargingTime;
        private Button _createTrasnportationDocBt;
        private Button _setTodayOrderBt;
        private Button _lastTransportationItemsBt;
        private LastTrasnportationsDocsAdapter _lastTrasnportationsDocsAdapter;
        //private ClientsSpinnerAdapter _clientsSpinnerAdapter;
        private VehiclesSpinnerAdapter _vehiclesSpinnerAdapter;
        private ProductsListFatAdapter _productsListAdapter;
        private DateTime _dateSelected;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (_view != null) return _view;
            View view = inflater.Inflate(Resource.Layout.TransportationDocumentsFragment, container, false);

            _view = view;
            _guiasRv = view.FindViewById<RecyclerView>(Resource.Id.GuiasRv);
            _productsListRv = view.FindViewById<RecyclerView>(Resource.Id.productsListRv);
            _emptyLayout = view.FindViewById(Resource.Id.emptyLayout);
            //_clientSpinner = view.FindViewById<Spinner>(Resource.Id.clientSpinner);
            _vehicleSpinner = view.FindViewById<Spinner>(Resource.Id.vehicleSpinner);
            _chargingTime = view.FindViewById<TextView>(Resource.Id.chargingTime);
            _createTrasnportationDocBt = view.FindViewById<Button>(Resource.Id.createTrasnportationDoc);
            _setTodayOrderBt = view.FindViewById<Button>(Resource.Id.setTodayOrderBt);
            _lastTransportationItemsBt = view.FindViewById<Button>(Resource.Id.lastTransportationItemsBt);

            _guiasRv.SetLayoutManager(new LinearLayoutManager(Context));
            _productsListRv.SetLayoutManager(new LinearLayoutManager(Context));
            return view;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.CreateTransportationDocumentCommand.CanExecuteChanged += CreateTransportationDocumentCommandCanExecuteChanged;
            _chargingTime.Click += ChargingTimeClick;
            _createTrasnportationDocBt.Click += CreateTrasnportationDocBtClick;
            _setTodayOrderBt.Click += SetTodayOrderBtClick;
            _lastTransportationItemsBt.Click += LastTransportationItemsBtClick;
            //_clientSpinner.ItemSelected += ClientSpinnerItemSelected;
            _vehicleSpinner.ItemSelected += VehicleSpinnerItemSelected;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.CreateTransportationDocumentCommand.CanExecuteChanged -= CreateTransportationDocumentCommandCanExecuteChanged;
            _chargingTime.Click -= ChargingTimeClick;
            _createTrasnportationDocBt.Click -= CreateTrasnportationDocBtClick;
            _setTodayOrderBt.Click -= SetTodayOrderBtClick;
            _lastTransportationItemsBt.Click -= LastTransportationItemsBtClick;
            //_clientSpinner.ItemSelected -= ClientSpinnerItemSelected;
            _vehicleSpinner.ItemSelected -= VehicleSpinnerItemSelected;
        }

        private void VehicleSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.VehicleSelected = ViewModel.Vehicles[e.Position];
        }

        /*private void ClientSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.ClientSelected = ViewModel.ClientsList[e.Position];
        }*/

        private void LastTransportationItemsBtClick(object sender, EventArgs e)
        {
            ViewModel.UseLastProductsListCommand.Execute(null);
            CreateTransportationDocumentCommandCanExecuteChanged(null, null);
        }

        private void SetTodayOrderBtClick(object sender, EventArgs e)
        {
            ViewModel.UseTodayOrderCommand.Execute(null);
            CreateTransportationDocumentCommandCanExecuteChanged(null, null);
        }

        private void CreateTransportationDocumentCommandCanExecuteChanged(object sender, EventArgs e)
        {
            _createTrasnportationDocBt.Enabled = ViewModel.CreateTransportationDocumentCommand.CanExecute(null);
        }

        private void CreateTrasnportationDocBtClick(object sender, EventArgs e)
        {
            ViewModel.CreateTransportationDocumentCommand.Execute(null);
        }

        private void ChargingTimeClick(object sender, EventArgs e)
        {
            DatePickerDialog datePickerDialog = new DatePickerDialog(this.Context, DateSelectedCallback, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.AddDays(1).Day);
            datePickerDialog.Show();
        }

        private void DateSelectedCallback(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            _dateSelected = e.Date;

            // Get Current Time
            Calendar c = Calendar.Instance;
            var mHour = c.Get(CalendarField.HourOfDay);
            var mMinute = c.Get(CalendarField.Minute);

            // Launch Time Picker Dialog
            TimePickerDialog timePickerDialog = new TimePickerDialog(this.Context, TimeSelectedCallback, mHour, mMinute, true);
            timePickerDialog.Show();
        }

        private void TimeSelectedCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            _dateSelected = _dateSelected.Date;
            _dateSelected = _dateSelected.AddHours(e.HourOfDay);
            _dateSelected = _dateSelected.AddMinutes(e.Minute);

            _chargingTime.Text = _dateSelected.ToString("dd/MM/yyyy HH:mm");
            ViewModel.DateSelected = _dateSelected;
            CreateTransportationDocumentCommandCanExecuteChanged(null, null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.LastTrasnportationsDocs):
                    SetLastTrasnportationsDocsRv();
                    break;
                case nameof(ViewModel.Vehicles):
                    _vehiclesSpinnerAdapter = new VehiclesSpinnerAdapter(ViewModel.Vehicles);
                    _vehicleSpinner.Adapter = _vehiclesSpinnerAdapter;
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
            _guiasRv.SetAdapter(_lastTrasnportationsDocsAdapter);
        }
    }
}