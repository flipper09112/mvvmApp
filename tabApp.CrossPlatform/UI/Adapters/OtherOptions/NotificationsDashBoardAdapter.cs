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
using tabApp.Core.Models;
using tabApp.UI.Fragments.Global.Other;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.OtherOptions
{
    public class NotificationsDashBoardAdapter : RecyclerView.Adapter
    {
        private List<NotificationItem<INotificationItem>> _notificationListItems;
        private NotificationsDashBoardViewModel _viewModel;

        public NotificationsDashBoardAdapter(List<NotificationItem<INotificationItem>> notificationListItems, NotificationsDashBoardViewModel viewModel)
        {
            _notificationListItems = notificationListItems;
            _viewModel = viewModel;
        }
        public override int ItemCount => _notificationListItems?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is HomePageOrderViewHolderAndroidX vh)
            {
                vh.Bind(
                    (((InternalClassOrder)_notificationListItems[holder.AdapterPosition].Data).Client, ((InternalClassOrder)_notificationListItems[holder.AdapterPosition].Data).ExtraOrder),
                    _viewModel);
            }
            else if(holder is NotificationItemViewHolder notvh)
            {
                notvh.Bind(
                    (Core.Models.Notifications.Notification)((InternalNotification)_notificationListItems[holder.AdapterPosition].Data).Notification,
                    _viewModel.GetClient(((InternalNotification)_notificationListItems[holder.AdapterPosition].Data).Notification.ClientId)
                    );
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if(_notificationListItems[viewType].Data is InternalClassOrder)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomePagerOrderItem, parent, false);
                return new HomePageOrderViewHolderAndroidX(view);
            }
            else if (_notificationListItems[viewType].Data is InternalNotification)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NotificationItem, parent, false);
                return new NotificationItemViewHolder(view);
            }

            return null;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }
    }
}