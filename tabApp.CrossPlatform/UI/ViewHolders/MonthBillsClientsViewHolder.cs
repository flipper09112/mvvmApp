using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.General;

namespace tabApp.UI.ViewHolders
{
    public class MonthBillsClientsViewHolder : RecyclerView.ViewHolder
    {
        private CardView _card;
        private TextView _clientName;
        private TextView _clientDesc;
        private ImageView _phoneIcon;
        private ImageView _locationIcon;
        private CheckBox _checkbox;
        private Client client;
        private Selectable<Client> data;

        public MonthBillsClientsViewHolder(View itemView) : base(itemView)
        {
            _card = itemView.FindViewById<CardView>(Resource.Id.card);
            _clientName = itemView.FindViewById<TextView>(Resource.Id.clientName);
            _clientDesc = itemView.FindViewById<TextView>(Resource.Id.clientDesc);
            _phoneIcon = itemView.FindViewById<ImageView>(Resource.Id.phoneIcon);
            _locationIcon = itemView.FindViewById<ImageView>(Resource.Id.locationIcon);
            _checkbox = itemView.FindViewById<CheckBox>(Resource.Id.checkbox);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;

            _checkbox.Click -= ItemViewClick;
            _checkbox.Click += ItemViewClick;
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            data.Selected = !data.Selected;

            _checkbox.Checked = data.Selected;
        }

        internal void Bind(Selectable<Client> data)
        {
            this.data = data;
            this.client = data.Data;

            _clientName.Text = client.Name;
            _clientDesc.Text = "Id: " + client.Id + "\nMorada" + client.Address.AddressDesc;

            _checkbox.Checked = data.Selected;

            SetColorBackground();

            SetIcons();
        }

        private void SetIcons()
        {
            if (client.PhoneNumber == null || client.PhoneNumber.Equals("") || client.PhoneNumber.Equals("Sem numero"))
            {
                _phoneIcon.SetImageResource(Resource.Drawable.ic_no_phone);
                _phoneIcon.SetColorFilter(client.Active ? GetColor(Resource.Color.red) : GetColor(Resource.Color.black));
            }
            else
            {
                _phoneIcon.SetImageResource(Resource.Drawable.ic_has_phone);
                _phoneIcon.SetColorFilter(client.PaymentType == PaymentTypeEnum.Semanal ? GetColor(Resource.Color.green) : GetColor(Resource.Color.black));
            }

            if (client.Address.Coordenadas == null || client.Address.Coordenadas.Equals("null"))
            {
                _locationIcon.SetImageResource(Resource.Drawable.ic_no_location);
                _locationIcon.SetColorFilter(client.Active ? GetColor(Resource.Color.red) : GetColor(Resource.Color.black));
            }
            else
            {
                _locationIcon.SetImageResource(Resource.Drawable.ic_client_location);
                _locationIcon.SetColorFilter(client.PaymentType == PaymentTypeEnum.Semanal ? GetColor(Resource.Color.green) : GetColor(Resource.Color.black));
            }
        }

        private Android.Graphics.Color GetColor(int color)
        {
            return CrossCurrentActivity.Current.AppContext.Resources.GetColor(color);
        }

        private void SetColorBackground()
        {
            if (!client.Active)
            {
                _card.SetBackgroundResource(Resource.Color.bg_Inative);
                return;
            }

            switch (client.PaymentType)
            {
                case PaymentTypeEnum.Diario:
                    _card.SetBackgroundResource(Resource.Color.bg_Diario);
                    break;
                case PaymentTypeEnum.Semanal:
                    _card.SetBackgroundResource(Resource.Color.bg_Semanal);
                    break;
                case PaymentTypeEnum.Mensal:
                    _card.SetBackgroundResource(Resource.Color.bg_Mensal);
                    break;
                case PaymentTypeEnum.JuntaDias:
                    _card.SetBackgroundResource(Resource.Color.bg_JuntaDias);
                    break;
                case PaymentTypeEnum.Loja:
                    _card.SetBackgroundResource(Resource.Color.bg_LojaSemana);
                    break;
            }
        }
    }
}