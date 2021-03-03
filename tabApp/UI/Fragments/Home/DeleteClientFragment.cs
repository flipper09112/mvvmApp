using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Home;

namespace tabApp.UI.Fragments.Home
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class DeleteClientFragment : BaseFragment<DeleteClientViewModel>
    {
        private MainActivity _activity;
        private TextView _name;
        private TextView _address;
        private TextView _nickName;
        private Button _confirmButton;
        private Button _backButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DeleteClientFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _name = view.FindViewById<TextView>(Resource.Id.name);
            _address = view.FindViewById<TextView>(Resource.Id.address);
            _nickName = view.FindViewById<TextView>(Resource.Id.nickName);
            _confirmButton = view.FindViewById<Button>(Resource.Id.confirmButton);
            _backButton = view.FindViewById<Button>(Resource.Id.backButton);

            return view;
        }
        public override void CleanBindings()
        {
            ViewModel.GoBack -= GoBack;
            _confirmButton.Click -= ConfirmButtonClick;
            _backButton.Click -= BackButtonClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            _name.Text = "Nome: " + ViewModel.Client.Name;
            _address.Text = "Morada: " + ViewModel.Client.Address.AddressDesc;
            _nickName.Text = "Alcunha: " + ViewModel.Client.SubName;
        }

        public override void SetupBindings()
        {
            ViewModel.GoBack += GoBack;
            _confirmButton.Click += ConfirmButtonClick;
            _backButton.Click += BackButtonClick;
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ConfirmButtonClick(object sender, EventArgs e)
        {
            ViewModel.DeleteCommand.Execute(null);
        }
    }
}