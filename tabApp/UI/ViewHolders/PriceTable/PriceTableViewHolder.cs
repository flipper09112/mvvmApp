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
        private CardView _card;
        private ImageView _productImage;
        private TextView _productName;
        private TextView _priceLabel;

        public int[] itemColors = {
            Resource.Color.blue, 
            Resource.Color.red, 
            Resource.Color.purple, 
            Resource.Color.maroon, 
            Resource.Color.lime50,
            Resource.Color.silver,
            Resource.Color.aqua50,
            Resource.Color.olive,
            Resource.Color.teal,
            Resource.Color.navy,
        };
        public PriceTableViewHolder(View itemView) : base(itemView)
        {
            _card = itemView.FindViewById<CardView>(Resource.Id.card);
            _productImage = itemView.FindViewById<ImageView>(Resource.Id.productImage);
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _priceLabel = itemView.FindViewById<TextView>(Resource.Id.priceLabel);
        }

        internal void Bind(Product product, bool hasFilter, Client clientFilter)
        {
            _productName.Text = product.Name;

            if (product.Unity)
            { 
                if(!hasFilter) _priceLabel.Text = product.PVP.ToString("C");
                else _priceLabel.Text = product.ReSaleValues.Find(item => item.ClientId == clientFilter.Id).Value.ToString("C");
            }
            else
            {
                if (!hasFilter) _priceLabel.Text = product.PVP.ToString("C") + "/Kg";
                else _priceLabel.Text = product.ReSaleValues.Find(item => item.ClientId == clientFilter.Id).Value.ToString("C") + "/Kg";
            }

            SetBackground(product.ProductType);
        }

        private void SetBackground(ProductTypeEnum productType)
        {
            switch(productType)
            {
                case ProductTypeEnum.Padaria:
                    _card.SetBackgroundResource(itemColors[0]);
                    break;
                case ProductTypeEnum.PastelariaIndividual:
                    _card.SetBackgroundResource(itemColors[1]);
                    break;
                case ProductTypeEnum.PastelariaIndividualSalgada:
                    _card.SetBackgroundResource(itemColors[2]);
                    break;
                case ProductTypeEnum.SemiFrioFamiliar:
                    _card.SetBackgroundResource(itemColors[3]);
                    break;
                case ProductTypeEnum.SemiFrioIndividual:
                    _card.SetBackgroundResource(itemColors[4]);
                    break;
                case ProductTypeEnum.Sortido:
                    _card.SetBackgroundResource(itemColors[5]);
                    break;
                case ProductTypeEnum.Tartes:
                    _card.SetBackgroundResource(itemColors[6]);
                    break;
                case ProductTypeEnum.Tortas:
                    _card.SetBackgroundResource(itemColors[7]);
                    break;
                case ProductTypeEnum.BolosTradicionais:
                    _card.SetBackgroundResource(itemColors[8]);
                    break;
                case ProductTypeEnum.BolosFestivos:
                    _card.SetBackgroundResource(itemColors[9]);
                    break;
            }
        }
    }
}