using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.RecyclerView.Widget;
using tabApp.Core.ViewModels.Main;

namespace tabApp.UI.ViewHolders.Settings
{
    public class DateSelectSettingItemViewHolder : RecyclerView.ViewHolder
    {
        private Context _context;
        private ImageView _icon;
        private TextView _title;
        private TextView _setting;
        private DateSelectSettingItem _itemSelected;

        public DateSelectSettingItemViewHolder(View itemView) : base(itemView)
        {
            _context = itemView.Context;

            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _title = itemView.FindViewById<TextView>(Resource.Id.title);
            _setting = itemView.FindViewById<TextView>(Resource.Id.setting);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            _itemSelected.Command.Execute(null);
        }

        internal void Bind(DateSelectSettingItem dateSelectSettingItem)
        {
            _itemSelected = dateSelectSettingItem;

            int id = _context.Resources.GetIdentifier(dateSelectSettingItem.ImageName, "drawable", _context.PackageName);
            _icon.SetImageResource(id);

            _title.Text = "Data de ultima alteração de preços";
            _setting.Text = dateSelectSettingItem.CurrentValue?.ToString("dd/MM/yyyy") ?? "No data";
        }
    }
}