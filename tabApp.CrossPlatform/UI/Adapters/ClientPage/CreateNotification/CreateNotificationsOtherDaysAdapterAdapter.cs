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
using tabApp.UI.ViewHolders.CreateNotifications;

namespace tabApp.UI.Adapters.ClientPage.CreateNotification
{
    class CreateNotificationsOtherDaysAdapterAdapter : RecyclerView.Adapter
    {
        private List<DateTime> extraDays;

        public CreateNotificationsOtherDaysAdapterAdapter(List<DateTime> extraDays)
        {
            this.extraDays = extraDays;
        }

        public override int ItemCount => extraDays?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CreateNotificationsOtherDaysViewHolder createNotificationsOtherDaysViewHolder)
            {
                createNotificationsOtherDaysViewHolder.Bind(extraDays[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemText, parent, false);
            return new CreateNotificationsOtherDaysViewHolder(view);
        }
    }
}