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
using tabApp.Core.Models.GlobalOrder;

namespace tabApp.UI.ViewHolders
{
    public class IndividualCakeOrderItemViewHolder : RecyclerView.ViewHolder
    {
        private TextView _clientName;
        private TextView _productsList;
        private Button _rightButton;
        private Button _dontSendOrderButton;

        public IndividualCakeOrderItemViewHolder(View itemView) : base(itemView)
        {
            _clientName = itemView.FindViewById<TextView>(Resource.Id.clientName);
            _productsList = itemView.FindViewById<TextView>(Resource.Id.productsList);
            _rightButton = itemView.FindViewById<Button>(Resource.Id.rightButton);
            _dontSendOrderButton = itemView.FindViewById<Button>(Resource.Id.dontSendOrderButton);
        }

        internal void Bind(CakeClientItem cakeClientItem)
        {
            _clientName.Text = cakeClientItem.Client.Name;
            SetOrderDesc(cakeClientItem.Products);
        }

        private void SetOrderDesc(List<(string ProductName, int Ammount)> products)
        {
            string desc = "";
            foreach(var item in products)
            {
                desc += item.ProductName + "-" + item.Ammount + "\n";
            }
            _productsList.Text = desc;
        }
    }
}