using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Other;

namespace tabApp.UI.Fragments.Global
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class DatabaseManagerPageFragment : BaseFragment<DatabaseManagerPageViewModel>
    {
        private MainActivity _activity;
        private CardView _sendDbCard;
        private CardView _restoreDbCard;
        private TextView _databaseDate;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DatabaseManagerPageFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _sendDbCard = view.FindViewById<CardView>(Resource.Id.sendDbCard);
            _restoreDbCard = view.FindViewById<CardView>(Resource.Id.restoreDbCard);
            _databaseDate = view.FindViewById<TextView>(Resource.Id.databaseDate);

            return view;
        }

        public override async void SetUI()
        {
            _databaseDate.Text = await SecureStorageHelper.GetKeyAsync(SecureStorageHelper.DatabaseDateDownloadKey) ?? "Sem data registada";
        }

        public override void SetupBindings()
        {
            _sendDbCard.Click += SendDbCardClick;
            _restoreDbCard.Click += RestoreDbCardClick;
            ViewModel.GoBack2Times += GoBack2Times;
            ViewModel.UpdateDownloadPercentage += UpdateDownloadPercentage;
        }

        public override void CleanBindings()
        {
            _sendDbCard.Click -= SendDbCardClick;
            _restoreDbCard.Click -= RestoreDbCardClick;
            ViewModel.GoBack2Times -= GoBack2Times;
            ViewModel.UpdateDownloadPercentage -= UpdateDownloadPercentage;
        }

        private void UpdateDownloadPercentage(object sender, EventArgs e)
        {
        }

        private void GoBack2Times(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

    private void SendDbCardClick(object sender, EventArgs e)
        {
            ViewModel.SendDatabaseCommand.Execute();
        }

        private void RestoreDbCardClick(object sender, EventArgs e)
        {
            ViewModel.RestoreDatabaseCommand.Execute();
        }
    }
}