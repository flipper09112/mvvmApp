using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tabApp.Core;
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters;
using tabApp.UI.Adapters.Home;
using tabApp.UI.Adapters.Swipe;

namespace tabApp.UI
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _clientsList;
        private ViewPager _homeViewPager;
        private TabLayout _tabLayout;
        private ClientsListAdapter _clientsAdapter;
        private HomePageViewPagerAdapter _viewPagerAdapter;
        private AlertDialog _longPressPopUp;
        private GridView _popUpGv;
        private ImageView _loadingImagePopUp;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _clientsList = view.FindViewById<RecyclerView>(Resource.Id.clientsList);
            _homeViewPager = view.FindViewById<ViewPager>(Resource.Id.homeViewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);

            _clientsList.SetLayoutManager(new LinearLayoutManager(Context));

            SwipeController swipeController = new SwipeController(Context, ViewModel);
            swipeController.AttachToRecyclerView(_clientsList);

            _viewPagerAdapter = new HomePageViewPagerAdapter(ChildFragmentManager);
            _viewPagerAdapter.ViewModel = ViewModel;
            _homeViewPager.Adapter = _viewPagerAdapter;
            _tabLayout.SetupWithViewPager(_homeViewPager, true); 


            return view;
        }

        public override void SetUI()
        {
            _activity.ShowToolbar();

            _clientsAdapter = new ClientsListAdapter(ViewModel.ClientsList, ViewModel.ShowClientPage, ViewModel.LongClickClient);
            _clientsList.SetAdapter(_clientsAdapter);
            _viewPagerAdapter.TabsOptions = ViewModel.TabsOptions;
            _viewPagerAdapter.NotifyDataSetChanged();
            SetupTabLayout();

            if(ViewModel.ClientSelected != null)
            {
                int pos = ViewModel.ClientsList.IndexOf(ViewModel.ClientSelected);
                _clientsList.ScrollToPosition(pos);
            }
        }
        private void SetupTabLayout()
        {
            List<string> tabsNames = new List<string>();
            _tabLayout.RemoveAllTabs();
            foreach (var tab in ViewModel?.TabsOptions)
            {
                _tabLayout.AddTab(_tabLayout.NewTab().SetText(tab.Name + " (" + tab.Count + ")"));
                tabsNames.Add(tab.ToString());
            }
            //_viewPagerAdapter.Title = tabsNames;
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.DeleteClientEvent += DeleteClientEvent;
            ViewModel.ShowOptionsLongPress += ShowOptionsLongPress;
            ViewModel.UpdateOrderList += UpdateOrderList;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.DeleteClientEvent -= DeleteClientEvent;
            ViewModel.ShowOptionsLongPress -= ShowOptionsLongPress;
            ViewModel.UpdateOrderList -= UpdateOrderList;
        }

        private void UpdateOrderList(object sender, EventArgs e)
        {
            _viewPagerAdapter.UpdateOrdersList();
        }

        private void ShowOptionsLongPress(object sender, EventArgs e)
        {
            _longPressPopUp = new AlertDialog.Builder(Context)
                                                        .SetView(GetChoiceView())
                                                        .Create();

            _longPressPopUp.Show();
        }

        private View GetChoiceView()
        {
            View view = LayoutInflater.From(Context).Inflate(Resource.Layout.dialog_choice, null);
            _popUpGv = (GridView)view.FindViewById(Resource.Id.gv_choice);

            _loadingImagePopUp = view.FindViewById<ImageView>(Resource.Id.custom_loading_imageView);
            Glide.With(Context)
                    .Load(Resource.Drawable.loading)
                    .Into(_loadingImagePopUp);

            //GridView data source, directly loaded from strings.xml
            List<LongPressItem> data = ViewModel.LongPressItemsList;

            //Custom adapter
            LongPressPopPupAdapter adapter;
            //Judgment type, load data source settings Adapter
            adapter = new LongPressPopPupAdapter(data, CloseLongPressPopUp);
            _popUpGv.Adapter = adapter;
            //Set the default selection
            //adapter.(eventSelected);

            return view;
        }

        internal void UpdateAllLists()
        {
            _viewPagerAdapter.TabsOptions = ViewModel.GetTabsOptions();
            _viewPagerAdapter?.UpdateAllLists();
        }

        private void CloseLongPressPopUp()
        {
            _longPressPopUp?.Dismiss();
        }

        private void DeleteClientEvent(object sender, EventArgs e)
        {
            Toast.MakeText(Context, "Delete", ToastLength.Long).Show();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }
    }

}