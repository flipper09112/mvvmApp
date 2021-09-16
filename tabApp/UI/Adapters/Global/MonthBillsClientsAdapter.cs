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
using tabApp.Core.Models;
using tabApp.Core.Models.General;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.Global
{
    public class MonthBillsClientsAdapter : RecyclerView.Adapter
    {
        private List<Selectable<Client>> monthClients;

        public MonthBillsClientsAdapter(List<Selectable<Client>> monthClients)
        {
            this.monthClients = monthClients;
        }

        public override int ItemCount => monthClients?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is MonthBillsClientsViewHolder monthBillsClientsVH)
            {
                monthBillsClientsVH.Bind(monthClients[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MonthBillsClients, parent, false);
            return new MonthBillsClientsViewHolder(view);
        }
    }
}