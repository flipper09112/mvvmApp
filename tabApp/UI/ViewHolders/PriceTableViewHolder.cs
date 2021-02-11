using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.UI.ViewHolders
{
    public class PriceTableViewHolder : RecyclerView.ViewHolder
    {
        private ImageView _productImage;
        private TextView _productName;
        private TextView _priceLabel;

        public PriceTableViewHolder(View itemView) : base(itemView)
        {
            _productImage = itemView.FindViewById<ImageView>(Resource.Id.productImage);
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _priceLabel = itemView.FindViewById<TextView>(Resource.Id.priceLabel);
        }

        internal void Bind(Product product)
        {
            _productName.Text = product.Name;

            if(product.Unity)
                _priceLabel.Text = product.PVP.ToString("C");
            else
                _priceLabel.Text = product.PVP.ToString("C") + "/Kg";
        }
    }
}