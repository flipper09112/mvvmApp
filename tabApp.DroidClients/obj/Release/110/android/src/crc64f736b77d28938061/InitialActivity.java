package crc64f736b77d28938061;


public class InitialActivity
	extends crc648d9adcc6b772c31e.MvxAppCompatActivity_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("tabApp.DroidClients.UI.InitialActivity, tabApp.DroidClients", InitialActivity.class, __md_methods);
	}


	public InitialActivity ()
	{
		super ();
		if (getClass () == InitialActivity.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.InitialActivity, tabApp.DroidClients", "", this, new java.lang.Object[] {  });
	}


	public InitialActivity (int p0)
	{
		super (p0);
		if (getClass () == InitialActivity.class)
			mono.android.TypeManager.Activate ("tabApp.DroidClients.UI.InitialActivity, tabApp.DroidClients", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

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
