using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;

namespace tabApp.UI.Adapters.Home
{
    public class LongPressPopPupAdapter : BaseAdapter
    {
        public override int Count => data?.Count ?? 0;

        private Action closeLongPressPopUp;
        private Action closeLoadingView;
        private List<LongPressItem> data;

        public LongPressPopPupAdapter(List<LongPressItem> data, Action closeLongPressPopUp)
        {
            this.closeLongPressPopUp = closeLongPressPopUp;
            this.data = data;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if(convertView == null)
            {
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.choice_item, parent, false);
            }
            var title = (TextView)convertView.FindViewById(Resource.Id.tv_choice);
            title.Text = data[position].Name;

            title.Click -= TitleClick;
            title.Click += TitleClick;

            return convertView;
        }

        private void TitleClick(object sender, EventArgs e)
        {
            TextView textView = (TextView)sender;

            var dataSelected = data.Find(data => data.Name.Equals(textView.Text));
            int pos = data.IndexOf(dataSelected);

            data[pos].Command?.Execute();
            closeLongPressPopUp?.Invoke();
        }
    }
}