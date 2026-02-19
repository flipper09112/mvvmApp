using AndroidX.RecyclerView.Widget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Models;

namespace tabApp.UI.ViewHolders
{
    public class NotificationItemViewHolder : RecyclerView.ViewHolder
    {
        private TextView _cardTitle;
        private TextView _info;
        private TextView _dateLabel;
        private ImageView _cardImage;

        public NotificationItemViewHolder(View itemView) : base(itemView)
        {
            _cardTitle = itemView.FindViewById<TextView>(Resource.Id.cardTitle);
            _info = itemView.FindViewById<TextView>(Resource.Id.info);
            _dateLabel = itemView.FindViewById<TextView>(Resource.Id.dateLabel);
            _cardImage = itemView.FindViewById<ImageView>(Resource.Id.cardImage);
        }

        internal void Bind(Notification notification, Client notificationClient)
        {
            _cardTitle.Text = "Cliente \"" + notificationClient?.Name + "\"\nId: " + notificationClient?.Id;
            _info.Text = notification.Info;
            _dateLabel.Text = notification.AlertDay.ToString("dd/MM/yyyy");
        }
    }
}