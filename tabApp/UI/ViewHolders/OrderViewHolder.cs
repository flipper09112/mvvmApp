using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using tabApp.Core;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;

namespace tabApp.UI.ViewHolders
{
    public class OrderViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private TextView _orderTitle;
        private TextView _orderDayLabel;
        private TextView _orderLabel;
        private TextView _productsLabel;
        private ImageView _imageView;
        private Button _cancelAppointment;
        private TextView _orderStatus;
        private ClientPageViewModel viewModel;
        private HomeViewModel homeViewModel;
        private ExtraOrder extraOrder;

        public OrderViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;

            _orderTitle = itemView.FindViewById<TextView>(Resource.Id.orderTitle);
            _orderDayLabel = itemView.FindViewById<TextView>(Resource.Id.orderDayLabel);
            _orderLabel = itemView.FindViewById<TextView>(Resource.Id.orderLabel);
            _productsLabel = itemView.FindViewById<TextView>(Resource.Id.productsLabel);
            _imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            _cancelAppointment = itemView.FindViewById<Button>(Resource.Id.cancelAppointment);
            _orderStatus = itemView.FindViewById<TextView>(Resource.Id.orderStatus);
        }

        internal void Bind(ExtraOrder extraOrder, Core.ViewModels.ClientPageViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.extraOrder = extraOrder;

            _orderDayLabel.Text = extraOrder.OrderDay.ToString("dd/MM/yyyy");
            _productsLabel.Text = viewModel.GetOrderDesc(extraOrder);
            _orderTitle.Text = extraOrder.IsTotal ? "Encomenda Total" : "Encomenda Extra";

            if (extraOrder.OrderDay.Date < DateTime.Today)
            {
                _imageView.SetColorFilter(GetColorFromInteger(Resource.Color.blue));
                _orderStatus.Text = "Concluída";
                _orderStatus.SetTextColor(GetColorFromInteger(Resource.Color.blue));

                if(extraOrder.StoreOrder)
                {
                    _cancelAppointment.Visibility = ViewStates.Visible;
                    _cancelAppointment.Text = "Anular registo";
                    _orderDayLabel.Text = "Registo do dia\n" + extraOrder.OrderDay.ToString("dd/MM/yyyy");

                    _cancelAppointment.Click -= CancelAppointmentClick;
                    _cancelAppointment.Click += CancelAppointmentClick;
                }
                else
                {
                    if(extraOrder.OrderDay.Date > viewModel.Client.PaymentDate.Date && !(extraOrder.AmmountedAdded ?? false))
                    {
                        _cancelAppointment.Visibility = ViewStates.Visible;
                        _cancelAppointment.SetBackgroundResource(Resource.Drawable.blue_button);
                        _cancelAppointment.Text = "Adicionar Extra";

                        _cancelAppointment.Click -= AddExtraAction;
                        _cancelAppointment.Click += AddExtraAction;
                    }
                    else
                    {
                        _cancelAppointment.Visibility = ViewStates.Invisible;
                    }
                }
            } else
            {
                _imageView.SetColorFilter(GetColorFromInteger(Resource.Color.green)); 
                _orderStatus.Text = "Pendente";
                _orderStatus.SetTextColor(GetColorFromInteger(Resource.Color.green));
                _cancelAppointment.Visibility = ViewStates.Visible;

                _cancelAppointment.Click -= CancelAppointmentClick;
                _cancelAppointment.Click += CancelAppointmentClick;
            }

        }

        private void AddExtraAction(object sender, EventArgs e)
        {
            viewModel.AddExtraFromOrderCommand.Execute(extraOrder);
        }

        public Color GetColorFromInteger(int colorId)
        {
            int color = context.GetColor(colorId);
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private void CancelAppointmentClick(object sender, EventArgs e)
        {
            viewModel.CancelOrderCommand.Execute(extraOrder);
        }

    }
}