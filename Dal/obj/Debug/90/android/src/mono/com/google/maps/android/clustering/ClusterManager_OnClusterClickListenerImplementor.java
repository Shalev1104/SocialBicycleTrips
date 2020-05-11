package mono.com.google.maps.android.clustering;


public class ClusterManager_OnClusterClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.maps.android.clustering.ClusterManager.OnClusterClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClusterClick:(Lcom/google/maps/android/clustering/Cluster;)Z:GetOnClusterClick_Lcom_google_maps_android_clustering_Cluster_Handler:Com.Google.Maps.Android.Clustering.ClusterManager/IOnClusterClickListenerInvoker, GoogleMapsUtilityBinding\n" +
			"";
		mono.android.Runtime.register ("Com.Google.Maps.Android.Clustering.ClusterManager+IOnClusterClickListenerImplementor, GoogleMapsUtilityBinding", ClusterManager_OnClusterClickListenerImplementor.class, __md_methods);
	}


	public ClusterManager_OnClusterClickListenerImplementor ()
	{
		super ();
		if (getClass () == ClusterManager_OnClusterClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Google.Maps.Android.Clustering.ClusterManager+IOnClusterClickListenerImplementor, GoogleMapsUtilityBinding", "", this, new java.lang.Object[] {  });
	}


	public boolean onClusterClick (com.google.maps.android.clustering.Cluster p0)
	{
		return n_onClusterClick (p0);
	}

	private native boolean n_onClusterClick (com.google.maps.android.clustering.Cluster p0);

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
