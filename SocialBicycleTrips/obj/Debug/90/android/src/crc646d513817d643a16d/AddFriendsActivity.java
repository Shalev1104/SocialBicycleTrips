package crc646d513817d643a16d;


public class AddFriendsActivity
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
		mono.android.Runtime.register ("SocialBicycleTrips.Activities.AddFriendsActivity, SocialBicycleTrips", AddFriendsActivity.class, __md_methods);
	}


	public AddFriendsActivity ()
	{
		super ();
		if (getClass () == AddFriendsActivity.class)
			mono.android.TypeManager.Activate ("SocialBicycleTrips.Activities.AddFriendsActivity, SocialBicycleTrips", "", this, new java.lang.Object[] {  });
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
