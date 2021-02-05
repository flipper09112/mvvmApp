using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.GlobalOrder;

namespace tabApp.UI.ViewHolders
{
    public class IndividualCakeOrderItemViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private ConstraintLayout _mainLayout;
        private TextView _clientName;
        private TextView _productsList;
        private Button _rightButton;
        private Button _dontSendOrderButton;

        public CakeClientItem Item { get; private set; }

        public IndividualCakeOrderItemViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;
            _mainLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.mainLayout);
            _clientName = itemView.FindViewById<TextView>(Resource.Id.clientName);
            _productsList = itemView.FindViewById<TextView>(Resource.Id.productsList);
            _rightButton = itemView.FindViewById<Button>(Resource.Id.rightButton);
            _dontSendOrderButton = itemView.FindViewById<Button>(Resource.Id.dontSendOrderButton);
        }

        internal void Bind(CakeClientItem cakeClientItem)
        {
            this.Item = cakeClientItem;

            _clientName.Text = cakeClientItem.Client.Name;
            SetOrderDesc(cakeClientItem.Products);

            if(cakeClientItem.Selected)
            {
                _mainLayout.SetBackgroundResource(Resource.Color.red50);
                _dontSendOrderButton.Text = "Encomendar";

            } else
            {
                _mainLayout.SetBackgroundResource(Resource.Drawable.CardViewBg);
                _dontSendOrderButton.Text = "Não encomendar";
            }

            _dontSendOrderButton.Click -= DontSendOrderButtonClick;
            _dontSendOrderButton.Click += DontSendOrderButtonClick;
        }

        private void SetOrderDesc(List<(string ProductName, int Ammount)> products)
        {
            string desc = "";
            foreach(var item in products)
            {
                desc += item.ProductName + " - " + item.Ammount + "\n";
            }
            _productsList.Text = desc;
        }

        private void DontSendOrderButtonClick(object sender, EventArgs e)
        {
            Item.Selected = !Item.Selected;
            Bind(Item);
        }
    }
}