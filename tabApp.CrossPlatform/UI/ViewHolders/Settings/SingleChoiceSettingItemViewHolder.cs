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
using tabApp.Core.ViewModels.Main;

namespace tabApp.UI.ViewHolders.Settings
{
    public class SingleChoiceSettingItemViewHolder : RecyclerView.ViewHolder
    {
        private ImageView _icon;
        private TextView _title;
        private TextView _setting;
        private SingleChoiceSettingItem<Delivery> _singleChoiceSettingItem;

        public SingleChoiceSettingItemViewHolder(View itemView) : base(itemView)
        {
            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _title = itemView.FindViewById<TextView>(Resource.Id.title);
            _setting = itemView.FindViewById<TextView>(Resource.Id.setting);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        internal void Bind(SingleChoiceSettingItem<Delivery> singleChoiceSettingItem)
        {
            _singleChoiceSettingItem = singleChoiceSettingItem;

            _setting.Text = _singleChoiceSettingItem.CurrentValue;
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            _singleChoiceSettingItem.Command.Execute(null);
        }
    }
}