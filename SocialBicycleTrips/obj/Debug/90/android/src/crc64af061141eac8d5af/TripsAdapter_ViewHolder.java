package crc64af061141eac8d5af;


public class TripsAdapter_ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Model.TripsAdapter+ViewHolder, SocialBicycleTrips", TripsAdapter_ViewHolder.class, __md_methods);
	}


	public TripsAdapter_ViewHolder ()
	{
		super ();
		if (getClass () == TripsAdapter_ViewHolder.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter+ViewHolder, SocialBicycleTrips", "", this, new java.lang.Object[] {  });
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
