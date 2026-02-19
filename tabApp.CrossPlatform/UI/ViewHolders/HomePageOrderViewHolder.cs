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
using tabApp.Core;
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
        private Button _addExtraButton;
        private HomeViewModel _viewModel;
        private (Client Client, ExtraOrder ExtraOrder) _extraOrder;

        public HomePageOrderViewHolder(View itemView) : base(itemView)
        {
            _orderTitle = itemView.FindViewById<TextView>(Resource.Id.orderTitle);
            _orderAddress = itemView.FindViewById<TextView>(Resource.Id.orderAddress);
            _orderDesc = itemView.FindViewById<TextView>(Resource.Id.orderDesc);
            _extraOrderTotal = itemView.FindViewById<TextView>(Resource.Id.extraOrderTotal);
            _addExtraButton = itemView.FindViewById<Button>(Resource.Id.addExtraButton);
        }

        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, Core.HomeViewModel viewModel)
        {
            _viewModel = viewModel;
            _extraOrder = extraOrder;

            _orderTitle.Text = "Encomenda para o cliente '"+ extraOrder.Client.Name + "'\nId: " + extraOrder.Client.Id;
            _orderAddress.Text = extraOrder.Client.Address.AddressDesc;
            _orderDesc.Text = viewModel.GetOrderDesc(extraOrder.ExtraOrder);
            _extraOrderTotal.Text = extraOrder.ExtraOrder.IsTotal ? "Total" : "Extra";
            _extraOrderTotal.SetTextColor(GetColorFromInteger(extraOrder.ExtraOrder.IsTotal ? Resource.Color.blue : Resource.Color.red));

            if(!(extraOrder.ExtraOrder.AmmountedAdded ?? false))
                _addExtraButton.Visibility = ViewStates.Visible;
            else
                _addExtraButton.Visibility = ViewStates.Invisible;

            _addExtraButton.Click -= AddExtraButtonClick;
            _addExtraButton.Click += AddExtraButtonClick;
        }

        private void AddExtraButtonClick(object sender, EventArgs e)
        {
            _viewModel.AddExtraFromOrderCommand.Execute(_extraOrder.ExtraOrder);
        }

        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, GlobalOrderViewModel viewModel)
        {
            _addExtraButton.Visibility = ViewStates.Invisible;
            _orderTitle.Text = "Encomenda para o cliente '" + extraOrder.Client.Name + "'\nId: " + extraOrder.Client.Id;
            _orderAddress.Text = extraOrder.Client.Address.AddressDesc;
            _orderDesc.Text = viewModel.GetOrderDesc(extraOrder.ExtraOrder);
            _extraOrderTotal.Text = extraOrder.ExtraOrder.IsTotal ? "Total" : "Extra";
            _extraOrderTotal.SetTextColor(GetColorFromInteger(extraOrder.ExtraOrder.IsTotal ? Resource.Color.blue : Resource.Color.red));
        }
        internal void Bind((Client Client, ExtraOrder ExtraOrder) extraOrder, SnoozeViewModel viewModel)
        {
            _addExtraButton.Visibility = ViewStates.Invisible;
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