package crc649ba559e34155f93c;


public class HomePageOrderViewHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("tabApp.UI.ViewHolders.HomePageOrderViewHolder, tabApp", HomePageOrderViewHolder.class, __md_methods);
	}


	public HomePageOrderViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == HomePageOrderViewHolder.class)
			mono.android.TypeManager.Activate ("tabApp.UI.ViewHolders.HomePageOrderViewHolder, tabApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
