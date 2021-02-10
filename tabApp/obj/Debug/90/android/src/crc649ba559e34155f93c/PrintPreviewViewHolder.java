package crc649ba559e34155f93c;


public class PrintPreviewViewHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("tabApp.UI.ViewHolders.PrintPreviewViewHolder, tabApp", PrintPreviewViewHolder.class, __md_methods);
	}


	public PrintPreviewViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == PrintPreviewViewHolder.class)
			mono.android.TypeManager.Activate ("tabApp.UI.ViewHolders.PrintPreviewViewHolder, tabApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
