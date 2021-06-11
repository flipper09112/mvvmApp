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
using tabApp.Core.ViewModels.ClientPage;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments.ClientPage
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class AddStoreRegistFragment : BaseFragment<AddStoreRegistViewModel>
    {
        private MainActivity _activity;
        private TextView _calendarLabel;
        private Button _saveButton;
        private Button _selectDayOrder;
        private Button _addProductButton;
        private RecyclerView _productsRecyclerView;
        private View _noItems;
        private ProductsAmmountListAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AddStoreRegistFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _calendarLabel = view.FindViewById<TextView>(Resource.Id.calendarLabel);
            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            _selectDayOrder = view.FindViewById<Button>(Resource.Id.selectDayOrder);
            _addProductButton = view.FindViewById<Button>(Resource.Id.addProductButton);
            _productsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.productsRecyclerView);
            _noItems = view.FindViewById<View>(Resource.Id.noItems);

            _productsRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }
        public override void CleanBindings()
        {
            _calendarLabel.Click -= CalendarLabelClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _addProductButton.Click -= AddProductButtonClick;
            _saveButton.Click -= SaveButtonClick;
            ViewModel.GoBack2Times -= GoBack2Times;
            _selectDayOrder.Click -= SelectDayOrderClick;
        }

        public override void SetUI()
        {
            _calendarLabel.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
            _noItems.Visibility = ViewModel.ItemsList.Count > 0 ? ViewStates.Invisible : ViewStates.Visible;

            _adapter = new ProductsAmmountListAdapter(ViewModel.ItemsList, ViewModel.SaveRegistCommand);
            _productsRecyclerView.SetAdapter(_adapter);

            SaveRegistCommandCanExecuteChanged(null, null);
        }

        public override void SetupBindings()
        {
            _calendarLabel.Click += CalendarLabelClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _addProductButton.Click += AddProductButtonClick;
            _saveButton.Click += SaveButtonClick;
            ViewModel.SaveRegistCommand.CanExecuteChanged += SaveRegistCommandCanExecuteChanged;
            ViewModel.GoBack2Times += GoBack2Times;
            _selectDayOrder.Click += SelectDayOrderClick;
        }

        private void SelectDayOrderClick(object sender, EventArgs e)
        {
            ViewModel.SelectDayCommand.Execute();
        }

        private void GoBack2Times(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void SaveRegistCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if(ViewModel.SaveRegistCommand.CanExecute(null))
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
            ViewModel.SaveRegistCommand.Execute(null);
        }

        private void AddProductButtonClick(object sender, EventArgs e)
        {
            ViewModel.AddProductCommand.Execute(null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.DateSelectedDailyOrder):
                case nameof(ViewModel.DateSelected):
                    SetUI();
                    break;
            }
        }

        private void CalendarLabelClick(object sender, EventArgs e)
        {
            ViewModel.ShowCalendarPickerCommand.Execute();
        }
    }
}