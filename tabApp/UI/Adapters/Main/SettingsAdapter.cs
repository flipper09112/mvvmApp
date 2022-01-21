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
using tabApp.Core.ViewModels.Main;
using tabApp.UI.ViewHolders.Settings;

namespace tabApp.UI.Adapters.Main
{
    public class SettingsAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => settingsList?.Count ?? 0;

        private List<SettingItem> settingsList;

        public SettingsAdapter(List<SettingItem> settingsList)
        {
            this.settingsList = settingsList;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is SingleChoiceSettingItemViewHolder singleChoiceSettingItemViewHolder)
            {
                singleChoiceSettingItemViewHolder.Bind((SingleChoiceSettingItem<Delivery>)settingsList[holder.AdapterPosition]);
            }
            else if(holder is SettingItemTitleViewHolder settingItemTitleViewHolder)
            {
                settingItemTitleViewHolder.Bind((SettingItemTitle)settingsList[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if(settingsList[viewType] is SingleChoiceSettingItem<Delivery> deliverySetting)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SingleChoiceSettingItem, parent, false);
                return new SingleChoiceSettingItemViewHolder(view);
            }

            else if (settingsList[viewType] is SettingItemTitle settingItemTitle)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SettingItemTitle, parent, false);
                return new SettingItemTitleViewHolder(view);
            }

            return null;
        }
        public override int GetItemViewType(int position)
        {
            return position;
        }
    }
}