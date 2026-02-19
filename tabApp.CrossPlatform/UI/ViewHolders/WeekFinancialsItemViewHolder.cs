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
using tabApp.Core.ViewModels.Global.Other.Finance;

namespace tabApp.UI.ViewHolders
{
    public class WeekFinancialsItemViewHolder : RecyclerView.ViewHolder
    {
        private TextView _dayLabel;
        private TextView _valueLabel;

        public WeekFinancialsItemViewHolder(View itemView) : base(itemView)
        {
            _dayLabel = itemView.FindViewById<TextView>(Resource.Id.dayLabel);
            _valueLabel = itemView.FindViewById<TextView>(Resource.Id.valueLabel);
        }

        internal void Bind(DayAmmount dayAmmount)
        {
            _dayLabel.Text = dayAmmount.Name;
            _valueLabel.Text = dayAmmount.Value.ToString("C");
        }
    }
}