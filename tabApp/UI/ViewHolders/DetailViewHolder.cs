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
    public class DetailViewHolder : RecyclerView.ViewHolder
    {
        private ImageView _imageView;
        private TextView _title;
        private TextView _detailDesc;
        private TextView _registDate;

        public DetailViewHolder(View itemView) : base(itemView)
        {
            _imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            _title = itemView.FindViewById<TextView>(Resource.Id.title);
            _detailDesc = itemView.FindViewById<TextView>(Resource.Id.detailDesc);
            _registDate = itemView.FindViewById<TextView>(Resource.Id.registDate);
        }

        internal void Bind(Regist regist)
        {
            _title.Text = regist.DetailType.ToString();
            _detailDesc.Text = regist.Info;
            SetImageIcon(regist.DetailType);
            _registDate.Text = regist.DetailRegistDay.ToString("dd/MM/yyyy");
        }

        private void SetImageIcon(DetailTypeEnum detailType)
        {
            switch(detailType)
            {
                case DetailTypeEnum.Payment:
                    _imageView.SetImageResource(Resource.Drawable.ic_payment);
                    break;
                case DetailTypeEnum.AddExtra:
                    _imageView.SetImageResource(Resource.Drawable.ic_add_extra);
                    break;
                case DetailTypeEnum.CancelOrder:
                    _imageView.SetImageResource(Resource.Drawable.ic_cancel_order);
                    break;
                case DetailTypeEnum.Edit:
                    _imageView.SetImageResource(Resource.Drawable.ic_edit);
                    break;
                default:
                    _imageView.SetImageResource(Resource.Drawable.ic_other_detail);
                    break;
            }
        }
    }
}