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
using tabApp.Core.ViewModels.Main;
using tabApp.UI.Adapters.Main;

namespace tabApp.UI.Fragments.Main
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _settingsRv;
        private SettingsAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SettingsFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _settingsRv = view.FindViewById<RecyclerView>(Resource.Id.settingsRv);


            return view;
        }

        public override void SetUI()
        {
            _settingsRv.SetLayoutManager(new LinearLayoutManager(Context));
            _adapter = new SettingsAdapter(ViewModel.SettingsList);
            _settingsRv.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            ViewModel.ChooseDeliveryEvent += ChooseDeliveryEvent;
            ViewModel.ReloadUIEvent += ReloadUIEvent;
        }
        public override void CleanBindings()
        {
            ViewModel.ReloadUIEvent -= ReloadUIEvent;
            ViewModel.ChooseDeliveryEvent -= ChooseDeliveryEvent;
        }

        private void ReloadUIEvent(object sender, EventArgs e)
        {
            _adapter.NotifyDataSetChanged();
        }

        private void ChooseDeliveryEvent(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Context);
            builder.SetTitle("Escolha um distribuidor");
            builder.SetItems(ViewModel.DeliveriesList, SelectDelivery);
            builder.Show();
        }

        private void SelectDelivery(object sender, DialogClickEventArgs e)
        {
            ViewModel.ChooseDeliveryIndexCommand.Execute(e.Which);
        }
    }
}