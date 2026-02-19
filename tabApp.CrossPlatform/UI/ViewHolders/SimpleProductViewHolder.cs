using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Helpers;

namespace tabApp.UI.ViewHolders
{
    public class SimpleProductViewHolder : RecyclerView.ViewHolder
    {
        public MvxCommand<Product> Click { get; internal set; }
        private TextView _productName;
        private Product product;

        public SimpleProductViewHolder(View itemView) : base(itemView)
        {
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            Click?.Execute(product);
        }

        internal void Bind(Product product, string searchWord)
        {
            this.product = product;
            _productName.Text = product.Name;
        }
    }
}