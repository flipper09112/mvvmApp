package crc64729f5a32a5dadc2d;


public abstract class BaseFragment_1
	extends crc64729f5a32a5dadc2d.BaseFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("tabApp.UI.BaseFragment`1, tabApp.DroidClients", BaseFragment_1.class, __md_methods);
	}


	public BaseFragment_1 ()
	{
		super ();
		if (getClass () == BaseFragment_1.class)
			mono.android.TypeManager.Activate ("tabApp.UI.BaseFragment`1, tabApp.DroidClients", "", this, new java.lang.Object[] {  });
	}


	public BaseFragment_1 (int p0)
	{
		super (p0);
		if (getClass () == BaseFragment_1.class)
			mono.android.TypeManager.Activate ("tabApp.UI.BaseFragment`1, tabApp.DroidClients", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
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
