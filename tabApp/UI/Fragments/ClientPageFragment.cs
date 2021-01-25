using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ClientPageFragment : BaseFragment<ClientPageViewModel>
    {
        private MainActivity _activity;

        #region Private Labels
        private TextView _clientName;
        private TextView _payDate;
        private Spinner _spinnerDates;
        private TextView _ammountToPay;
        private Button _payButton;
        private Button _addExtraButton;
        private Button _addOrderButton;
        private Button _editButton;
        private Button _optionsButton;
        private ViewPager _viewPager;
        private TabLayout _tabLayout;
        private ClientPageViewPagerAdapter _viewPagerAdapter;
        private TextView _segLabel;
        private TextView _segValue;
        private TextView _terLabel;
        private TextView _terValue;
        private TextView _quaLabel;
        private TextView _quaValue;
        private TextView _quiLabel;
        private TextView _quiValue;
        private TextView _sexLabel;
        private TextView _sexValue;
        private TextView _sabLabel;
        private TextView _sabValue;
        private TextView _domLabel;
        private TextView _domValue;
        private TextView _extLabel;
        private TextView _extValue;
        private ClientPageSpinnerAdapter _spinnerAdapter;
        #endregion

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ClientPageFragment, container, false);

            _activity = ParentActivity as MainActivity;

            #region Setup Labels
            _clientName = view.FindViewById<TextView>(Resource.Id.clientName);
            _payDate = view.FindViewById<TextView>(Resource.Id.payDate);
            _spinnerDates = view.FindViewById<Spinner>(Resource.Id.spinnerDates);
            _ammountToPay = view.FindViewById<TextView>(Resource.Id.ammountToPay);
            _payButton = view.FindViewById<Button>(Resource.Id.payButton);
            _addExtraButton = view.FindViewById<Button>(Resource.Id.addExtraButton);
            _addOrderButton = view.FindViewById<Button>(Resource.Id.addOrderButton);
            _editButton = view.FindViewById<Button>(Resource.Id.editButton);
            _optionsButton = view.FindViewById<Button>(Resource.Id.optionsButton);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);

            _segLabel = view.FindViewById<TextView>(Resource.Id.segLabel);
            _segValue = view.FindViewById<TextView>(Resource.Id.segValue);
            _terLabel = view.FindViewById<TextView>(Resource.Id.terLabel);
            _terValue = view.FindViewById<TextView>(Resource.Id.terValue);
            _quaLabel = view.FindViewById<TextView>(Resource.Id.quaLabel);
            _quaValue = view.FindViewById<TextView>(Resource.Id.quaValue);
            _quiLabel = view.FindViewById<TextView>(Resource.Id.quiLabel);
            _quiValue = view.FindViewById<TextView>(Resource.Id.quiValue);
            _sexLabel = view.FindViewById<TextView>(Resource.Id.sexLabel);
            _sexValue = view.FindViewById<TextView>(Resource.Id.sexValue);
            _sabLabel = view.FindViewById<TextView>(Resource.Id.sabLabel);
            _sabValue = view.FindViewById<TextView>(Resource.Id.sabValue);
            _domLabel = view.FindViewById<TextView>(Resource.Id.domLabel);
            _domValue = view.FindViewById<TextView>(Resource.Id.domValue);
            _extLabel = view.FindViewById<TextView>(Resource.Id.extLabel);
            _extValue = view.FindViewById<TextView>(Resource.Id.extValue);

            _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            #endregion

            _viewPagerAdapter = new ClientPageViewPagerAdapter(ViewModel.TabsOptions, ViewModel.Client);
            _viewPager.Adapter = _viewPagerAdapter;
            _tabLayout.SetupWithViewPager(_viewPager, true);

            _spinnerAdapter = new ClientPageSpinnerAdapter(ViewModel.SpinnerDates);
            _spinnerDates.Adapter = _spinnerAdapter;

            return view;
        }

        #region UI
        public override void SetUI()
        {
            _activity.HideToolbar();

            _clientName.Text = ViewModel.Client.Name;
            _payDate.Text = ViewModel.PayDate;
            _segLabel.Text = ViewModel.SegLabel;
            _terLabel.Text = ViewModel.TerLabel;
            _quaLabel.Text = ViewModel.QuaLabel;
            _quiLabel.Text = ViewModel.QuiLabel;
            _sexLabel.Text = ViewModel.SexLabel;
            _sabLabel.Text = ViewModel.SabLabel;
            _domLabel.Text = ViewModel.DomLabel;
            _extLabel.Text = ViewModel.ExtLabel;

            _segValue.Text = ViewModel.SegValue;
            _terValue.Text = ViewModel.TerValue;
            _quaValue.Text = ViewModel.QuaValue;
            _quiValue.Text = ViewModel.QuiValue;
            _sexValue.Text = ViewModel.SexValue;
            _sabValue.Text = ViewModel.SabValue;
            _domValue.Text = ViewModel.DomValue;
            _extValue.Text = ViewModel.ExtValue;

            _ammountToPay.Text = ViewModel.AmmountToPay;
            _payButton.Text = ViewModel.PayButtonText;
            _addExtraButton.Text = ViewModel.AddExtraButtonText;
            _addOrderButton.Text = ViewModel.AddOrderButtonText;
            _editButton.Text = ViewModel.EditButtonText;
            _optionsButton.Text = ViewModel.OptionsButtonText;

            PayCommandCanExecuteChanged(null, null);

            SetupTabLayout();
        }


        private void SetupTabLayout()
        {
            List<string> tabsNames = new List<string>();
            _tabLayout.RemoveAllTabs();
            foreach(var tab in ViewModel.TabsOptions)
            {
                _tabLayout.AddTab(_tabLayout.NewTab().SetText(tab.ToString()));
                tabsNames.Add(tab.ToString());
            }
            _viewPagerAdapter.Title = tabsNames;
        }
        #endregion

        public override void CleanBindings()
        {
            _spinnerDates.ItemSelected -= SinnerDatesItemSelected;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _payButton.Click -= PayButtonClick;
            ViewModel.PayCommand.CanExecuteChanged -= PayCommandCanExecuteChanged;
            _activity.ShowToolbar();
            _addExtraButton.Click -= AddExtraButtonClick;
        }

        public override void SetupBindings()
        {
            _spinnerDates.ItemSelected += SinnerDatesItemSelected;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _payButton.Click += PayButtonClick;
            ViewModel.PayCommand.CanExecuteChanged += PayCommandCanExecuteChanged;
            _addExtraButton.Click += AddExtraButtonClick;
        }


        #region Events

        private void AddExtraButtonClick(object sender, EventArgs e)
        {
            ViewModel.AddExtraCommand.Execute(null);
        }
        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void SinnerDatesItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.DateSelected = _spinnerAdapter[e.Position];
        }
        private void PayButtonClick(object sender, EventArgs e)
        {
            ViewModel.PayCommand.Execute(null);
        }
        private void PayCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.PayCommand.CanExecute(null))
                _payButton.Enabled = true;
            else
                _payButton.Enabled = false;
        }
        #endregion
    }
}