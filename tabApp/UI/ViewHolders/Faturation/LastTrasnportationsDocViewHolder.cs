using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models.Faturation;

namespace tabApp.UI.ViewHolders.Faturation
{
    public class LastTrasnportationsDocViewHolder : RecyclerView.ViewHolder
    {
        private View _mainView;
        private TextView _docName;
        private TextView _docDateTime;
        private ImageView _pdfIcon;
        private TrasnportationDoc _trasnportationDoc;
        private MvxCommand<TrasnportationDoc> _docClick;

        public LastTrasnportationsDocViewHolder(View itemView) : base(itemView)
        {
            _mainView = itemView;
            _docName = itemView.FindViewById<TextView>(Resource.Id.docName);
            _docDateTime = itemView.FindViewById<TextView>(Resource.Id.docDateTime);
            _pdfIcon = itemView.FindViewById<ImageView>(Resource.Id.pdfIcon);

            _pdfIcon.Click -= PdfIconClick;
            _pdfIcon.Click += PdfIconClick;
        }

        private void PdfIconClick(object sender, EventArgs e)
        {
            _docClick?.Execute(_trasnportationDoc);
        }

        public void Bind(TrasnportationDoc trasnportationDoc, MvxCommand<TrasnportationDoc> docClick)
        {
            _trasnportationDoc = trasnportationDoc;
            _docClick = docClick;

            _docName.Text = trasnportationDoc.Name;
            _docDateTime.Text = trasnportationDoc.EmissionDate.ToString("ddd dd/MM/yyyy");
        }
    }
}