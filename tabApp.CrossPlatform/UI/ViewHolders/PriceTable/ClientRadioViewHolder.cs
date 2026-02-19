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
using tabApp.Core.ViewModels.Global.PriceTable;

namespace tabApp.UI.ViewHolders.PriceTable
{
    public class ClientRadioViewHolder : RecyclerView.ViewHolder
    {
        private RadioButton _radioButton;
        private PriceTableFilterItemModel _priceTableFilterItemModel;
        private int _position;

        public ClientRadioViewHolder(View itemView) : base(itemView)
        {
            _radioButton = itemView.FindViewById<RadioButton>(Resource.Id.radioButton);

            _radioButton.Click -= RadioButtonClick;
            _radioButton.Click += RadioButtonClick;
        }

        internal void Bind(PriceTableFilterItemModel priceTableFilterItemModel, int position, Client clientSelected)
        {
            _priceTableFilterItemModel = priceTableFilterItemModel;
            _position = position;

            _radioButton.Text = priceTableFilterItemModel.ClientsList[position].Name;

            _radioButton.Checked = clientSelected?.Id == priceTableFilterItemModel.ClientsList[position].Id ? true : false;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            _priceTableFilterItemModel.Command.Execute(_priceTableFilterItemModel.ClientsList[_position]);
        }
    }
}