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

namespace tabApp.UI.ViewHolders
{
    public class AddItemReportViewHolder : RecyclerView.ViewHolder
    {
        private Button _addButton;

        public Action Click { get; set; }

        public AddItemReportViewHolder(View itemView) : base(itemView)
        {
            _addButton = itemView.FindViewById<Button>(Resource.Id.addButton);

            _addButton.Click -= AddButtonClick;
            _addButton.Click += AddButtonClick;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            Click?.Invoke();
        }
    }
}