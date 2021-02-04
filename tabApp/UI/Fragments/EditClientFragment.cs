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
using tabApp.UI.Adapters.EditClient;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class EditClientFragment : BaseFragment<EditClientViewModel>
    {
        private MainActivity _activity;
        private Button _saveChangesButton;
        private ViewPager _editViewPager;
        private TabLayout _tabLayout;
        private EditClientViewPagerAdapter _viewPagerAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.EditClientFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _saveChangesButton = view.FindViewById<Button>(Resource.Id.saveChangesButton);
            _editViewPager = view.FindViewById<ViewPager>(Resource.Id.editViewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);

            _viewPagerAdapter = new EditClientViewPagerAdapter(ChildFragmentManager);
            _viewPagerAdapter.ViewModel = ViewModel;
            _editViewPager.Adapter = _viewPagerAdapter;
            _tabLayout.SetupWithViewPager(_editViewPager, true);

            SetupTabLayout();
            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            _saveChangesButton.Enabled = ViewModel.SaveChangesCommand.CanExecute(null);
        }

        private void SetupTabLayout()
        {
            List<string> tabsNames = new List<string>();
            _tabLayout.RemoveAllTabs();
            foreach (var tab in ViewModel.TabsOptions)
            {
                _tabLayout.AddTab(_tabLayout.NewTab().SetText(tab.ToString()));
                tabsNames.Add(tab.ToString());
            }
            _viewPagerAdapter.Title = tabsNames;
        }

        public override void CleanBindings()
        {
            ViewModel.SaveChangesCommand.CanExecuteChanged -= SaveChangesCommandCanExecuteChanged;
            _saveChangesButton.Click -= SaveChangesButtonClick;
        }
        public override void SetupBindings()
        {
            ViewModel.SaveChangesCommand.CanExecuteChanged += SaveChangesCommandCanExecuteChanged;
            _saveChangesButton.Click += SaveChangesButtonClick;
        }

        private void SaveChangesButtonClick(object sender, EventArgs e)
        {
            if(ViewModel.SaveChangesCommand.CanExecute(null))
            {
                ViewModel.SaveChangesCommand.Execute(null);
            }
        }

        private void SaveChangesCommandCanExecuteChanged(object sender, EventArgs e)
        {
            _saveChangesButton.Enabled = ViewModel.SaveChangesCommand.CanExecute(null);
        }
    }
}