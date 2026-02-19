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

namespace tabApp.UI.ViewHolders.CreateNotifications
{
    public class CreateNotificationsOtherDaysViewHolder : RecyclerView.ViewHolder
    {
        private TextView _radioButton;
        private DateTime _date;

        public CreateNotificationsOtherDaysViewHolder(View itemView) : base(itemView)
        {
            _radioButton = itemView.FindViewById<TextView>(Resource.Id.radioButton);

            _radioButton.Click -= RadioButtonClick;
            _radioButton.Click += RadioButtonClick;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
        }

        internal void Bind(DateTime dateTime)
        {
            _date = dateTime;
            _radioButton.Text = dateTime.ToString("dd/MM/yyyy");
        }
    }
}