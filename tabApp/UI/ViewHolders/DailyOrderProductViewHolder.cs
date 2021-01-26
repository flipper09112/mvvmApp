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

namespace tabApp.UI.ViewHolders
{
    public class DailyOrderProductViewHolder : RecyclerView.ViewHolder
    {
        private TextView _productName;
        private TextView _productAmmount;

        public DailyOrderProductViewHolder(View itemView) : base(itemView)
        {
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _productAmmount = itemView.FindViewById<TextView>(Resource.Id.productAmmount);
        }

        internal void Bind((string ProductName, string Ammount) p)
        {
            _productName.Text = p.ProductName;
            _productAmmount.Text = p.Ammount;
        }
    }
}