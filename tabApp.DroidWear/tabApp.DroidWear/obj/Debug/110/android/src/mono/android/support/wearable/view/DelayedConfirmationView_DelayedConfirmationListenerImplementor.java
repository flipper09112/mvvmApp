package mono.android.support.wearable.view;


public class DelayedConfirmationView_DelayedConfirmationListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.DelayedConfirmationView.DelayedConfirmationListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimerFinished:(Landroid/view/View;)V:GetOnTimerFinished_Landroid_view_View_Handler:Android.Support.Wearable.Views.DelayedConfirmationView/IDelayedConfirmationListenerInvoker, Xamarin.Android.Wear\n" +
			"n_onTimerSelected:(Landroid/view/View;)V:GetOnTimerSelected_Landroid_view_View_Handler:Android.Support.Wearable.Views.DelayedConfirmationView/IDelayedConfirmationListenerInvoker, Xamarin.Android.Wear\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.DelayedConfirmationView+IDelayedConfirmationListenerImplementor, Xamarin.Android.Wear", DelayedConfirmationView_DelayedConfirmationListenerImplementor.class, __md_methods);
	}


	public DelayedConfirmationView_DelayedConfirmationListenerImplementor ()
	{
		super ();
		if (getClass () == DelayedConfirmationView_DelayedConfirmationListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.DelayedConfirmationView+IDelayedConfirmationListenerImplementor, Xamarin.Android.Wear", "", this, new java.lang.Object[] {  });
	}


	public void onTimerFinished (android.view.View p0)
	{
		n_onTimerFinished (p0);
	}

	private native void n_onTimerFinished (android.view.View p0);


	public void onTimerSelected (android.view.View p0)
	{
		n_onTimerSelected (p0);
	}

	private native void n_onTimerSelected (android.view.View p0);

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
