package crc64af061141eac8d5af;


public class TripsAdapter
	extends android.widget.ArrayAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"";
		mono.android.Runtime.register ("Model.TripsAdapter, SocialBicycleTrips", TripsAdapter.class, __md_methods);
	}


	public TripsAdapter (android.content.Context p0, int p1)
	{
		super (p0, p1);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public TripsAdapter (android.content.Context p0, int p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public TripsAdapter (android.content.Context p0, int p1, java.lang.Object[] p2)
	{
		super (p0, p1, p2);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:T[], Mono.Android", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public TripsAdapter (android.content.Context p0, int p1, int p2, java.lang.Object[] p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib:T[], Mono.Android", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public TripsAdapter (android.content.Context p0, int p1, java.util.List p2)
	{
		super (p0, p1, p2);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:System.Collections.Generic.IList`1<T>, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public TripsAdapter (android.content.Context p0, int p1, int p2, java.util.List p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == TripsAdapter.class)
			mono.android.TypeManager.Activate ("Model.TripsAdapter, SocialBicycleTrips", "Android.Content.Context, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib:System.Collections.Generic.IList`1<T>, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);

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
