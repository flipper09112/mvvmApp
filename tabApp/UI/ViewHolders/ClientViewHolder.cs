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
    public class ClientViewHolder : RecyclerView.ViewHolder
    {
        private CardView _card;
        private TextView _clientName;
        private TextView _clientDesc;
        private Client client;

        public Action<Client> Click; 

        public ClientViewHolder(View itemView) : base(itemView)
        {
            _card = itemView.FindViewById<CardView>(Resource.Id.card);
            _clientName = itemView.FindViewById<TextView>(Resource.Id.clientName);
            _clientDesc = itemView.FindViewById<TextView>(Resource.Id.clientDesc);

            itemView.Click -= ItemViewClick;
            itemView.Click += ItemViewClick;
        }

        internal void Bind(Client client)
        {
            this.client = client;

            _clientName.Text = client.Name;
            _clientDesc.Text = "Id: " + client.Id + "\nMorada" + client.Address.AddressDesc;

            SetColorBackground();
        }

        private void SetColorBackground()
        {
            if(!client.Active)
            {
                _card.SetBackgroundResource(Resource.Color.bg_Inative);
                return;
            }

            switch(client.PaymentType)
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
        private void ItemViewClick(object sender, EventArgs e)
        {
            Click?.Invoke(client);
        }
    }
}