using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels.EditClient;
using tabApp.UI.ViewHolders;
using tabApp.UI.ViewHolders.CreateNotifications;

namespace tabApp.UI.Adapters.ClientPage.CreateNotification
{
    public class CreateNotificationsPredefinedAdapter : RecyclerView.Adapter
    {
        private CreateNotificationsViewModel viewModel;
        private List<PreDefinedAlertItem> preDefinedAlertItems;

        public CreateNotificationsPredefinedAdapter(CreateNotificationsViewModel viewModel, List<PreDefinedAlertItem> preDefinedAlertItems)
        {
            this.viewModel = viewModel;
            this.preDefinedAlertItems = preDefinedAlertItems;
        }

        public override int ItemCount => preDefinedAlertItems?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is ItemRadioViewHolderAndroidX itemRadioViewHolderAndroidX)
            {
                itemRadioViewHolderAndroidX.Bind(preDefinedAlertItems[holder.AdapterPosition], viewModel.PreDefinedAlertSelected, viewModel.SelectPreDefinedAlertCommand);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemRadio, parent, false);
            return new ItemRadioViewHolderAndroidX(view);
        }
    }
}