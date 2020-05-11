package mono.com.google.maps.android.clustering;


public class ClusterManager_OnClusterInfoWindowClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.maps.android.clustering.ClusterManager.OnClusterInfoWindowClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClusterInfoWindowClick:(Lcom/google/maps/android/clustering/Cluster;)V:GetOnClusterInfoWindowClick_Lcom_google_maps_android_clustering_Cluster_Handler:Com.Google.Maps.Android.Clustering.ClusterManager/IOnClusterInfoWindowClickListenerInvoker, GoogleMapsUtilityBinding\n" +
			"";
		mono.android.Runtime.register ("Com.Google.Maps.Android.Clustering.ClusterManager+IOnClusterInfoWindowClickListenerImplementor, GoogleMapsUtilityBinding", ClusterManager_OnClusterInfoWindowClickListenerImplementor.class, __md_methods);
	}


	public ClusterManager_OnClusterInfoWindowClickListenerImplementor ()
	{
		super ();
		if (getClass () == ClusterManager_OnClusterInfoWindowClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Google.Maps.Android.Clustering.ClusterManager+IOnClusterInfoWindowClickListenerImplementor, GoogleMapsUtilityBinding", "", this, new java.lang.Object[] {  });
	}


	public void onClusterInfoWindowClick (com.google.maps.android.clustering.Cluster p0)
	{
		n_onClusterInfoWindowClick (p0);
	}

	private native void n_onClusterInfoWindowClick (com.google.maps.android.clustering.Cluster p0);

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
