package crc64b8b35f75a7a40178;


public class TripAdapter_TripsHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SocialBicycleTrips.Adapters.TripAdapter+TripsHolder, SocialBicycleTrips", TripAdapter_TripsHolder.class, __md_methods);
	}


	public TripAdapter_TripsHolder ()
	{
		super ();
		if (getClass () == TripAdapter_TripsHolder.class)
			mono.android.TypeManager.Activate ("SocialBicycleTrips.Adapters.TripAdapter+TripsHolder, SocialBicycleTrips", "", this, new java.lang.Object[] {  });
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
