using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using tabApp.Core;
using tabApp.Core.Models.Notifications;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.Home
{
    public class NotificationsListAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => listNotifications?.Count ?? 0;
        private List<Notification> listNotifications;
        private HomeViewModel viewModel;

        public NotificationsListAdapter(List<Notification> value, Core.HomeViewModel viewModel)
        {
            this.listNotifications = value;
            this.viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var notificationVH = holder as NotificationItemViewHolder;
            notificationVH.Bind(listNotifications[position], viewModel.GetClient(listNotifications[position].ClientId));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NotificationItem, parent, false);
            return new NotificationItemViewHolder(view);
        }
    }
}