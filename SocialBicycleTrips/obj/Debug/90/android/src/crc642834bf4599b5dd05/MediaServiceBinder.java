package crc642834bf4599b5dd05;


public class MediaServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SocialBicycleTrips.Services.MediaServiceBinder, SocialBicycleTrips", MediaServiceBinder.class, __md_methods);
	}


	public MediaServiceBinder ()
	{
		super ();
		if (getClass () == MediaServiceBinder.class)
			mono.android.TypeManager.Activate ("SocialBicycleTrips.Services.MediaServiceBinder, SocialBicycleTrips", "", this, new java.lang.Object[] {  });
	}

	public MediaServiceBinder (crc642834bf4599b5dd05.MediaService p0)
	{
		super ();
		if (getClass () == MediaServiceBinder.class)
			mono.android.TypeManager.Activate ("SocialBicycleTrips.Services.MediaServiceBinder, SocialBicycleTrips", "SocialBicycleTrips.Services.MediaService, SocialBicycleTrips", this, new java.lang.Object[] { p0 });
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
