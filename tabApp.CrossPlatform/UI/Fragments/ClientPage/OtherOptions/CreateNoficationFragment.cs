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
using tabApp.Core.ViewModels.ClientPage.OtherOptions;
using tabApp.UI.Adapters.OtherOptions.Notifications;

namespace tabApp.UI.Fragments.ClientPage.OtherOptions
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class CreateNoficationFragment : BaseFragment<CreateNoficationViewModel>
    {
        private MainActivity _activity;
        private GridView _gridViewViewNotifications;
        private CreateNoficationGridAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;
            View view = inflater.Inflate(Resource.Layout.CreateNoficationFragment, container, false);

            _gridViewViewNotifications = view.FindViewById<GridView>(Resource.Id.gridViewNotifications);

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            _adapter = new CreateNoficationGridAdapter(ViewModel.Options);
            _gridViewViewNotifications.Adapter = _adapter;
        }

        public override void CleanBindings()
        {
            ViewModel.GoBack -= GoBack;
        }

        public override void SetupBindings()
        {
            ViewModel.GoBack += GoBack;
        }

        private void GoBack(object sender, EventArgs e)
        {
            Activity.SupportFragmentManager.PopBackStack();
        }
    }
}