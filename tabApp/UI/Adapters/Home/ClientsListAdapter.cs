using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class ClientsListAdapter : RecyclerView.Adapter
    {
        public List<Client> ClientsList;
        private MvxAsyncCommand<Client> showClientPage;

        public ClientsListAdapter(List<Client> clientsList, MvxAsyncCommand<Client> showClientPage)
        {
            this.ClientsList = clientsList;
            this.showClientPage = showClientPage;
        }

        public override int ItemCount => ClientsList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ClientViewHolder clientVH = holder as ClientViewHolder;
            clientVH.Bind(ClientsList[holder.AdapterPosition]);
            clientVH.Click = ClickClient;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ClientListItem, parent, false);
            return new ClientViewHolder(view);
        }

        private void ClickClient(Client clienSelected)
        {
            showClientPage.Execute(clienSelected);
        }

    }
}