package crc64b84dadbf4acaf435;


public class ProductDetailsFragment
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
		mono.android.Runtime.register ("tabApp.DroidClients.UI.Fragments.Catalog.ProductDetailsFragment, tabApp.DroidClients", ProductDetailsFragment.class, __md_methods);
	}


	public ProductDetailsFragment ()
	{
		super ();
		if (getClass () == ProductDetailsFragment.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.Fragments.Catalog.ProductDetailsFragment, tabApp.DroidClients", "", this, new java.lang.Object[] {  });
	}


	public ProductDetailsFragment (int p0)
	{
		super (p0);
		if (getClass () == ProductDetailsFragment.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.Fragments.Catalog.ProductDetailsFragment, tabApp.DroidClients", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
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
