package crc6487812edc773d59ec;


public class InfoFragment
	extends crc64729f5a32a5dadc2d.BaseFragment_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("tabApp.DroidClients.UI.Fragments.Info.InfoFragment, tabApp.DroidClients", InfoFragment.class, __md_methods);
	}


	public InfoFragment ()
	{
		super ();
		if (getClass () == InfoFragment.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.Fragments.Info.InfoFragment, tabApp.DroidClients", "", this, new java.lang.Object[] {  });
	}


	public InfoFragment (int p0)
	{
		super (p0);
		if (getClass () == InfoFragment.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.Fragments.Info.InfoFragment, tabApp.DroidClients", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
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
