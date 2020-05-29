package crc64ac215617748f9b25;


public class NotificationHelper
	extends android.content.ContextWrapper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Helper.NotificationHelper, Helper", NotificationHelper.class, __md_methods);
	}


	public NotificationHelper (android.content.Context p0)
	{
		super (p0);
		if (getClass () == NotificationHelper.class)
			mono.android.TypeManager.Activate ("Helper.NotificationHelper, Helper", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
