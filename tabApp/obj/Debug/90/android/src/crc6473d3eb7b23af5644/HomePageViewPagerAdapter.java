package crc6473d3eb7b23af5644;


public class HomePageViewPagerAdapter
	extends androidx.fragment.app.FragmentStatePagerAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItem:(I)Landroidx/fragment/app/Fragment;:GetGetItem_IHandler\n" +
			"n_destroyItem:(Landroid/view/View;ILjava/lang/Object;)V:GetDestroyItem_Landroid_view_View_ILjava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("tabApp.UI.Adapters.Home.HomePageViewPagerAdapter, tabApp", HomePageViewPagerAdapter.class, __md_methods);
	}


	public HomePageViewPagerAdapter (androidx.fragment.app.FragmentManager p0)
	{
		super (p0);
		if (getClass () == HomePageViewPagerAdapter.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Adapters.Home.HomePageViewPagerAdapter, tabApp", "AndroidX.Fragment.App.FragmentManager, Xamarin.AndroidX.Fragment", this, new java.lang.Object[] { p0 });
	}


	public HomePageViewPagerAdapter (androidx.fragment.app.FragmentManager p0, int p1)
	{
		super (p0, p1);
		if (getClass () == HomePageViewPagerAdapter.class)
			mono.android.TypeManager.Activate ("tabApp.UI.Adapters.Home.HomePageViewPagerAdapter, tabApp", "AndroidX.Fragment.App.FragmentManager, Xamarin.AndroidX.Fragment:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public androidx.fragment.app.Fragment getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native androidx.fragment.app.Fragment n_getItem (int p0);


	public void destroyItem (android.view.View p0, int p1, java.lang.Object p2)
	{
		n_destroyItem (p0, p1, p2);
	}

	private native void n_destroyItem (android.view.View p0, int p1, java.lang.Object p2);

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
