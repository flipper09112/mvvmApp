using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class ClientOrderAdapter : RecyclerView.Adapter
    {
        private List<ProductAmmount> orderProducts;
        private MvxCommand saveNewOrderCommand;
        private int _itemCount;

        public ClientOrderAdapter(List<ProductAmmount> orderProducts, MvvmCross.Commands.MvxCommand saveNewOrderCommand)
        {
            this.orderProducts = orderProducts;
            this.saveNewOrderCommand = saveNewOrderCommand;
            _itemCount = orderProducts.Count == 0 ? 1 : orderProducts.Count;
        }

        public override int ItemCount => _itemCount;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is EmptyListViewHolder)
            {
                var vh = holder as EmptyListViewHolder;
            }
            if (holder is ProductsOrderListViewHolder)
            {
                var vh = holder as ProductsOrderListViewHolder;
                vh.Bind(orderProducts[holder.AdapterPosition]);
                vh.SaveButtonCanChange = saveNewOrderCommand;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (orderProducts.Count == 0)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EmptyListItems, parent, false);
                return new EmptyListViewHolder(view);

            } else
            {
                var inflater = LayoutInflater.From(parent.Context);
                var view = inflater.Inflate(Resource.Layout.ProductOrderItem, parent, false);
                return new ProductsOrderListViewHolder(view);
            }
        }
    }
}