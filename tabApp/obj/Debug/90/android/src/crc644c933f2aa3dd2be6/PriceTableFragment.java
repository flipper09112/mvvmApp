package crc644c933f2aa3dd2be6;


public class PriceTableFragment
	extends crc642076a8bd1dc4bf73.BaseFragment_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("tabApp.UI.Fragments.Global.PriceTableFragment, tabApp", PriceTableFragment.class, __md_methods);
	}


	public PriceTableFragment ()
	{
		super ();
		if (getClass () == PriceTableFragment.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Fragments.Global.PriceTableFragment, tabApp", "", this, new java.lang.Object[] {  });
	}


	public PriceTableFragment (int p0)
	{
		super (p0);
		if (getClass () == PriceTableFragment.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Fragments.Global.PriceTableFragment, tabApp", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
