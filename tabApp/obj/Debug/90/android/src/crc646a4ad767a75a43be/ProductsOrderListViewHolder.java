package crc646a4ad767a75a43be;


public class ProductsOrderListViewHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("tabApp.UI.Adapters.ProductsOrderListViewHolder, tabApp", ProductsOrderListViewHolder.class, __md_methods);
	}


	public ProductsOrderListViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == ProductsOrderListViewHolder.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Adapters.ProductsOrderListViewHolder, tabApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
