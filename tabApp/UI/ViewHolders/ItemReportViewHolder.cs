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

namespace tabApp.UI.ViewHolders
{
    public class ItemReportViewHolder : RecyclerView.ViewHolder
    {
        private TextView _productName;
        private Button _removeProductBt;
        private Product _product;

        public Action<Product> Click { get; set; }

        public ItemReportViewHolder(View itemView) : base(itemView)
        {
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _removeProductBt = itemView.FindViewById<Button>(Resource.Id.removeProductBt);

            _removeProductBt.Click -= RemoveProductBtClick;
            _removeProductBt.Click += RemoveProductBtClick;
        }

        internal void Bind(Product product)
        {
            this._product = product;

            _productName.Text = product.Name;
        }
        private void RemoveProductBtClick(object sender, EventArgs e)
        {
            Click?.Invoke(_product);
        }
    }
}