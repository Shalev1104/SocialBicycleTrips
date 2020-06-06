package crc646d513817d643a16d;


public class MyTripsActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SocialBicycleTrips.Activities.MyTripsActivity, SocialBicycleTrips", MyTripsActivity.class, __md_methods);
	}


	public MyTripsActivity ()
	{
		super ();
		if (getClass () == MyTripsActivity.class)
			mono.android.TypeManager.Activate ("SocialBicycleTrips.Activities.MyTripsActivity, SocialBicycleTrips", "", this, new java.lang.Object[] {  });
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
