using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global;
using tabApp.UI.Adapters.Global;
using tabApp.UI.Adapters.Home;

namespace tabApp.UI.Fragments.Global
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PriceTableFragment : BaseFragment<PriceTableViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _recyclerView;
        private EditText _searchEdt;
        private TextView _resultsLabel;
        private ImageView _filterIcon;
        private ImageView _configIcon;
        private PriceTableAdapter _adapter;
        private AlertDialog _longPressPopUp;
        private GridView _popUpGv;
        private ImageView _loadingImagePopUp;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PriceTableFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _searchEdt = view.FindViewById<EditText>(Resource.Id.editMobileNo);
            _resultsLabel = view.FindViewById<TextView>(Resource.Id.resultsLabel);
            _filterIcon = view.FindViewById<ImageView>(Resource.Id.filterIcon);
            _configIcon = view.FindViewById<ImageView>(Resource.Id.configIcon);

            _adapter = new PriceTableAdapter(ViewModel, ViewModel.AllProducts);
            _recyclerView.SetLayoutManager(new GridLayoutManager(Context, 3, GridLayoutManager.Vertical, false));
            _recyclerView.SetAdapter(_adapter);

            return view;
        }

        public override void CleanBindings()
        {
            _activity.ShowToolbar();
            _activity.ShowMenu();
            _searchEdt.TextChanged -= SearchEdtClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _filterIcon.Click -= FilterIconClick;
            _configIcon.Click -= ConfigIconClick;
            ViewModel.ShowLongPressOptions -= ShowLongPressOptions;
        }

        public override void SetUI()
        {
            _adapter.AllProducts = ViewModel.AllProducts;
            _resultsLabel.Text = ViewModel.AllProducts.Count + " resultados encontrados";
            _filterIcon.SetImageResource(ViewModel.HasFilter ? Resource.Drawable.ic_filter_full : Resource.Drawable.ic_filter);
            _adapter.NotifyDataSetChanged();
        }

        public override void SetupBindings()
        {
            _activity.HideToolbar();
            _activity.HideMenu();
            _searchEdt.TextChanged += SearchEdtClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _filterIcon.Click += FilterIconClick;
            _configIcon.Click += ConfigIconClick;
            ViewModel.ShowLongPressOptions += ShowLongPressOptions;
        }

        private void ShowLongPressOptions(object sender, EventArgs e)
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
        private void CloseLongPressPopUp()
        {
            _longPressPopUp?.Dismiss();
        }

        private void ConfigIconClick(object sender, EventArgs e)
        {
            ViewModel.ShowPriceTableConfigurationCommand.Execute(null);
        }

        private void FilterIconClick(object sender, EventArgs e)
        {
            ViewModel.ShowPriceTableFilterCommand.Execute(null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void SearchEdtClick(object sender, EventArgs e)
        {
            ViewModel.SearchProduct = _searchEdt.Text;
        }
    }
}