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
using tabApp.Core.ViewModels.Main;

namespace tabApp.UI.ViewHolders.Settings
{
    public class SettingItemTitleViewHolder : RecyclerView.ViewHolder
    {
        private TextView _settingTitle;

        public SettingItemTitleViewHolder(View itemView) : base(itemView)
        {
            _settingTitle = itemView.FindViewById<TextView>(Resource.Id.settingTitle);
        }

        internal void Bind(SettingItemTitle settingItemTitle)
        {
            _settingTitle.Text = settingItemTitle.Title;
        }
    }
}