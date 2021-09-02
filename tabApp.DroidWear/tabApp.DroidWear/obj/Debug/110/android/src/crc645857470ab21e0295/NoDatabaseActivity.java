package crc645857470ab21e0295;


public class NoDatabaseActivity
	extends crc648d9adcc6b772c31e.MvxAppCompatActivity_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("tabApp.DroidWear.UI.Activities.Errors.NoDatabaseActivity, tabApp.DroidWear", NoDatabaseActivity.class, __md_methods);
	}


	public NoDatabaseActivity ()
	{
		super ();
		if (getClass () == NoDatabaseActivity.class)
			mono.android.TypeManager.Activate ("tabApp.DroidWear.UI.Activities.Errors.NoDatabaseActivity, tabApp.DroidWear", "", this, new java.lang.Object[] {  });
	}


	public NoDatabaseActivity (int p0)
	{
		super (p0);
		if (getClass () == NoDatabaseActivity.class)
			mono.android.TypeManager.Activate ("tabApp.DroidWear.UI.Activities.Errors.NoDatabaseActivity, tabApp.DroidWear", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
