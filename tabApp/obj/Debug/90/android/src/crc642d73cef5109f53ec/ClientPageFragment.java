package crc642d73cef5109f53ec;


public class ClientPageFragment
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
		mono.android.Runtime.register ("tabApp.UI.Fragments.ClientPageFragment, tabApp", ClientPageFragment.class, __md_methods);
	}


	public ClientPageFragment ()
	{
		super ();
		if (getClass () == ClientPageFragment.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Fragments.ClientPageFragment, tabApp", "", this, new java.lang.Object[] {  });
	}


	public ClientPageFragment (int p0)
	{
		super (p0);
		if (getClass () == ClientPageFragment.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Fragments.ClientPageFragment, tabApp", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
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