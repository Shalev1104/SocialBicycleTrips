package mono.com.google.android.libraries.places.widget.listener;


public class PlaceSelectionListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.libraries.places.widget.listener.PlaceSelectionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onError:(Lcom/google/android/gms/common/api/Status;)V:GetOnError_Lcom_google_android_gms_common_api_Status_Handler:Google.Places.IPlaceSelectionListenerInvoker, Xamarin.Google.Places\n" +
			"n_onPlaceSelected:(Lcom/google/android/libraries/places/api/model/Place;)V:GetOnPlaceSelected_Lcom_google_android_libraries_places_api_model_Place_Handler:Google.Places.IPlaceSelectionListenerInvoker, Xamarin.Google.Places\n" +
			"";
		mono.android.Runtime.register ("Google.Places.IPlaceSelectionListenerImplementor, Xamarin.Google.Places", PlaceSelectionListenerImplementor.class, __md_methods);
	}


	public PlaceSelectionListenerImplementor ()
	{
		super ();
		if (getClass () == PlaceSelectionListenerImplementor.class)
			mono.android.TypeManager.Activate ("Google.Places.IPlaceSelectionListenerImplementor, Xamarin.Google.Places", "", this, new java.lang.Object[] {  });
	}


	public void onError (com.google.android.gms.common.api.Status p0)
	{
		n_onError (p0);
	}

	private native void n_onError (com.google.android.gms.common.api.Status p0);


	public void onPlaceSelected (com.google.android.libraries.places.api.model.Place p0)
	{
		n_onPlaceSelected (p0);
	}

	private native void n_onPlaceSelected (com.google.android.libraries.places.api.model.Place p0);

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
