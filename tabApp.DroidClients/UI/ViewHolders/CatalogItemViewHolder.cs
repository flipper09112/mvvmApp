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
using tabApp.Core.ViewModelsClient.Catalog;

namespace tabApp.DroidClients.UI.ViewHolders
{
    public class CatalogItemViewHolder : RecyclerView.ViewHolder
    {
        private ImageView _image;
        private TextView _name;
        private CatalogTypeItem _catalogTypeItem;

        public CatalogItemViewHolder(View itemView) : base(itemView)
        {
            _image = itemView.FindViewById<ImageView>(Resource.Id.imageView3);
            _name = itemView.FindViewById<TextView>(Resource.Id.textView4);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        internal void Bind(CatalogTypeItem catalogTypeItem)
        {
            _catalogTypeItem = catalogTypeItem;

            _name.Text = catalogTypeItem.Name;
            _image.SetImageResource(GetResourceId(catalogTypeItem.IconName));
        }

        private int GetResourceId(string iconName)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            return act.Resources.GetIdentifier(iconName, "drawable", act.PackageName);
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            _catalogTypeItem.Command.Execute(_catalogTypeItem.Name);
        }
    }
}