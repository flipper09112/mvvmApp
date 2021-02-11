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
using tabApp.Core.ViewModels.Global;
using tabApp.UI.Adapters;
using tabApp.UI.Adapters.Home;

namespace tabApp.UI.Fragments.Global
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class GlobalOrderFragment : BaseFragment<GlobalOrderViewModel>
    {
        private RecyclerView _ordersRecyclerView;
        private RecyclerView _cakesRecyclerView;
        private RecyclerView _mainRecyclerView;
        private Button _otherOptions;
        private Button _sendOrder;
        private Button _addOrderCakes;
        private Button _addProduct;
        private View _noOrdersItems;
        private View _noCakesOrdersItems;
        private MainActivity _activity;
        private ProductsAmmountListAdapter _mainAdapter;
        private HomePageOrdersListAdapter _orderAdapter;
        private CakesTotalOrderAdapter _cakesAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.GlobalOrderFragment, container, false);

            _ordersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.ordersRecyclerView);
            _cakesRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.cakesRecyclerView);
            _mainRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.mainRecyclerView);
            _otherOptions = view.FindViewById<Button>(Resource.Id.otherOptions);
            _sendOrder = view.FindViewById<Button>(Resource.Id.sendOrder);
            _addOrderCakes = view.FindViewById<Button>(Resource.Id.addOrderCakes);
            _addProduct = view.FindViewById<Button>(Resource.Id.addProduct);
            _noOrdersItems = view.FindViewById<View>(Resource.Id.noOrdersItems);
            _noCakesOrdersItems = view.FindViewById<View>(Resource.Id.noCakesOrdersItems);

            _activity = ParentActivity as MainActivity;

            _mainRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _cakesRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _ordersRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));


            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _sendOrder.Click -= SendOrderClick;

            _activity.ShowMenu();
        }

        public override void SetUI()
        {
            _activity.HideMenu();
            _activity.HideToolbar();

            _mainAdapter = new ProductsAmmountListAdapter(ViewModel.ProductsList, null, true);
            _mainRecyclerView.SetAdapter(_mainAdapter);

            _orderAdapter = new HomePageOrdersListAdapter(ViewModel.TomorrowOrders, ViewModel);
            _ordersRecyclerView.SetAdapter(_orderAdapter);

            _cakesAdapter = new CakesTotalOrderAdapter(ViewModel.CakesClients, ViewModel);
            _cakesRecyclerView.SetAdapter(_cakesAdapter);

            _noOrdersItems.Visibility = ViewModel.TomorrowOrders?.Count == 0 ? ViewStates.Visible : ViewStates.Invisible;
            _noCakesOrdersItems.Visibility = ViewModel.CakesClients?.Count == 0 ? ViewStates.Visible : ViewStates.Invisible;
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _sendOrder.Click += SendOrderClick;
        }

        private void SendOrderClick(object sender, EventArgs e)
        {
            ViewModel.SaveAllDataCommand.Execute(null);
            string email = "encomendas@panilima.pt";
            string info = ViewModel.GetTextForSentToEmail();

            var emailIntent = new Intent(Android.Content.Intent.ActionSendto);
            emailIntent.PutExtra(Android.Content.Intent.ExtraEmail, new string[] {email});
            emailIntent.PutExtra(Android.Content.Intent.ExtraSubject, "Encomenda Manuela");
            emailIntent.PutExtra(Android.Content.Intent.ExtraText, info);
            emailIntent.SetType("message/rfc822");
            StartActivity(emailIntent);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }
    }
}