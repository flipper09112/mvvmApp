using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsClient;

namespace tabApp.DroidClients.UI.ViewHolders
{
    public class HomepageItemViewHolder : RecyclerView.ViewHolder
    {
        private ImageView _itemIcon;
        private TextView _itemName;
        private HomepageItem _homepageItem;

        public HomepageItemViewHolder(View itemView) : base(itemView)
        {
            _itemIcon = itemView.FindViewById<ImageView>(Resource.Id.itemIcon);
            _itemName = itemView.FindViewById<TextView>(Resource.Id.itemName);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        internal void Bind(HomepageItem homepageItem)
        {
            _homepageItem = homepageItem;

            _itemName.Text = homepageItem.Name;
            _itemIcon.SetImageResource(GetResourceId(homepageItem.IconName));
        }

        private int GetResourceId(string iconName)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            return act.Resources.GetIdentifier(iconName, "drawable", act.PackageName);
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            _homepageItem.Command?.Execute();
        }
    }
}