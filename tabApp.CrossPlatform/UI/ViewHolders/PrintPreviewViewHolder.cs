using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.UI.ViewHolders
{
    public class PrintPreviewViewHolder : RecyclerView.ViewHolder
    {
        private TextView _printPreviewLabel;
        private TextView _titleLabel;
        private Switch _switch;
        private ConstraintLayout _layout;
        private PrintPreview _printPreview;

        public PrintPreviewViewHolder(View itemView) : base(itemView)
        {
            _printPreviewLabel = itemView.FindViewById<TextView>(Resource.Id.printPreviewLabel);
            _titleLabel = itemView.FindViewById<TextView>(Resource.Id.titleLabel);
            _switch = itemView.FindViewById<Switch>(Resource.Id.switch1);
            _layout = itemView.FindViewById<ConstraintLayout>(Resource.Id.layout);

            _layout.Click -= ItemViewClick;
            _layout.Click += ItemViewClick;
        }

        private void ItemViewClick(object sender, EventArgs e)
        {
            _printPreview.Selected = !_printPreview.Selected;
            _printPreview.SetSelectedCommand.Execute(_printPreview);
        }

        internal void Bind(PrintPreview printPreview)
        {
            _printPreview = printPreview;

            _printPreviewLabel.Text = printPreview.Preview;
            _titleLabel.Text = printPreview.Name;
            _switch.Checked = printPreview.Selected;
        }
    }
}