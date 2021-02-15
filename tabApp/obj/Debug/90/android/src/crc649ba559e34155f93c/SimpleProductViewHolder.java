package crc649ba559e34155f93c;


public class SimpleProductViewHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("tabApp.UI.ViewHolders.SimpleProductViewHolder, tabApp", SimpleProductViewHolder.class, __md_methods);
	}


	public SimpleProductViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == SimpleProductViewHolder.class)
			mono.android.TypeManager.Activate ("tabApp.UI.ViewHolders.SimpleProductViewHolder, tabApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
