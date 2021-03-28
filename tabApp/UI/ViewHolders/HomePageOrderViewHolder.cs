using Android.App;
using Android.Content;
using Android.Graphics;
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
using tabApp.Core.ViewModels.Global;
using tabApp.Core.ViewModels.Snooze;
using tabApp.UI.Fragments.Snooze;

namespace tabApp.UI.ViewHolders
{
    public class HomePageOrderViewHolder : RecyclerView.ViewHolder
    {
        private TextView _orderTitle;
        private TextView _orderAddress;
        private TextView _orderDesc;
        private TextView _extraOrderTotal;

        public HomePageOrderViewHolder(View itemView) : base(itemView)
        {
            _orderTitle = itemView.FindViewById<TextView>(Resource.Id.orderTitle);
            _orderAddress = itemView.FindViewById<TextView>(Resource.Id.orderAddress);
            _orderDesc = itemView.FindViewById<TextView>(Resource.Id.orderDesc);
            _extraOrderTotal = itemView.FindViewById<TextView>(Resource.Id.extraOrderTotal);
        }

        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, Core.HomeViewModel viewModel)
        {
            _orderTitle.Text = "Encomenda para o cliente '"+ extraOrder.Client.Name + "'\nId: " + extraOrder.Client.Id;
            _orderAddress.Text = extraOrder.Client.Address.AddressDesc;
            _orderDesc.Text = viewModel.GetOrderDesc(extraOrder.ExtraOrder);
            _extraOrderTotal.Text = extraOrder.ExtraOrder.IsTotal ? "Total" : "Extra";
            _extraOrderTotal.SetTextColor(GetColorFromInteger(extraOrder.ExtraOrder.IsTotal ? Resource.Color.blue : Resource.Color.red));
        }

        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, GlobalOrderViewModel viewModel)
        {
            _orderTitle.Text = "Encomenda para o cliente '" + extraOrder.Client.Name + "'\nId: " + extraOrder.Client.Id;
            _orderAddress.Text = extraOrder.Client.Address.AddressDesc;
            _orderDesc.Text = viewModel.GetOrderDesc(extraOrder.ExtraOrder);
            _extraOrderTotal.Text = extraOrder.ExtraOrder.IsTotal ? "Total" : "Extra";
            _extraOrderTotal.SetTextColor(GetColorFromInteger(extraOrder.ExtraOrder.IsTotal ? Resource.Color.blue : Resource.Color.red));
        }
        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, SnoozeViewModel viewModel)
        {
            _orderTitle.Text = "Encomenda para o cliente '" + extraOrder.Client.Name + "'\nId: " + extraOrder.Client.Id;
            _orderAddress.Text = extraOrder.Client.Address.AddressDesc;
            _orderDesc.Text = viewModel.GetOrderDesc(extraOrder.ExtraOrder);
            _extraOrderTotal.Text = extraOrder.ExtraOrder.IsTotal ? "Total" : "Extra";
            _extraOrderTotal.SetTextColor(GetColorFromInteger(extraOrder.ExtraOrder.IsTotal ? Resource.Color.blue : Resource.Color.red));
        }
        public Color GetColorFromInteger(int colorId)
        {
            int color = MainActivity.Instance.ApplicationContext.GetColor(colorId);
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}