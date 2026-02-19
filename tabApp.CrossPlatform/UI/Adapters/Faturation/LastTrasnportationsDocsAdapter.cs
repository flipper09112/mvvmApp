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
using tabApp.UI.ViewHolders;
using tabApp.UI.ViewHolders.Faturation;

namespace tabApp.UI.Adapters.Faturation
{
    public class LastTrasnportationsDocsAdapter : RecyclerView.Adapter
    {
        public MvxCommand<TrasnportationDoc> DocClick { get; internal set; }
        private List<TrasnportationDoc> _lastTrasnportationsDocs;

        public LastTrasnportationsDocsAdapter(List<TrasnportationDoc> lastTrasnportationsDocs)
        {
            _lastTrasnportationsDocs = lastTrasnportationsDocs;
        }

        public override int ItemCount => _lastTrasnportationsDocs?.Count ?? 0;


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            LastTrasnportationsDocViewHolder lastTrasnportationsDoc = holder as LastTrasnportationsDocViewHolder;
            lastTrasnportationsDoc.Bind(_lastTrasnportationsDocs[holder.AdapterPosition], DocClick);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.LastTrasnportationsDocItem, parent, false);
            return new LastTrasnportationsDocViewHolder(view);
        }
    }
}