using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.ClientPage.OtherOptions;
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.ClientPage.OtherOptions
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PrintAccountFragment : BaseFragment<PrintAccountViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _printPreviewsList;
        private CardView _printButton;
        private Spinner _spinner;
        private TextView _dateSelected;
        private LinearLayout _android_custom_gridview_layout;
        private PrintPreviewAdapter _adapter;
        private PrintAccountSpinnerAdapter _spinnerAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PrintAccountFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _printPreviewsList = view.FindViewById<RecyclerView>(Resource.Id.printPreviewsList);
            _printButton = view.FindViewById<CardView>(Resource.Id.printButton);
            _spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            _dateSelected = view.FindViewById<TextView>(Resource.Id.dateSelected);
            _android_custom_gridview_layout = view.FindViewById<LinearLayout>(Resource.Id.android_custom_gridview_layout);

            _printPreviewsList.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false));

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.PrintCommand.CanExecuteChanged -= PrintCommandCanExecuteChanged;
            _printButton.Click -= PrintButtonClick;
            _dateSelected.Click -= DateSelectedClick;
        }

        public override void SetUI()
        {
            PrintCommandCanExecuteChanged(null, null);
            _dateSelected.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");

            _spinnerAdapter = new PrintAccountSpinnerAdapter(ViewModel.PairedDevices);
            _spinner.Adapter = _spinnerAdapter;

            _adapter = new PrintPreviewAdapter(ViewModel.PrintPreviewList);
            _printPreviewsList.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.PrintCommand.CanExecuteChanged += PrintCommandCanExecuteChanged;
            _printButton.Click += PrintButtonClick;
            _dateSelected.Click += DateSelectedClick;
        }

        private void DateSelectedClick(object sender, EventArgs e)
        {
            //ViewModel.ShowDatePickerCommand.Execute(null);
        }

        private void PrintCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.PrintCommand.CanExecute(null))
            {
                _printButton.Clickable = true;
                _android_custom_gridview_layout.SetBackgroundResource(Resource.Drawable.CardViewBg);
            }
            else
            {
                _printButton.Clickable = false;
                _android_custom_gridview_layout.SetBackgroundResource(Resource.Color.grey);
            }
        }

        private void PrintButtonClick(object sender, EventArgs e)
        {
            ViewModel.PrintCommand.Execute(null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewModel.PrintPreviewList):
                    _adapter.NotifyDataSetChanged();
                    break;
                case nameof(ViewModel.DateSelected):
                    _dateSelected.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
                    break; 
                case nameof(ViewModel.PairedDevices):
                    _spinnerAdapter = new PrintAccountSpinnerAdapter(ViewModel.PairedDevices);
                    _spinner.Adapter = _spinnerAdapter;
                    break;
            }
        }
    }
}