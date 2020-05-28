package mono.com.android.volley;


public class RequestQueue_RequestFinishedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.android.volley.RequestQueue.RequestFinishedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRequestFinished:(Lcom/android/volley/Request;)V:GetOnRequestFinished_Lcom_android_volley_Request_Handler:Volley.RequestQueue/IRequestFinishedListenerInvoker, Xamarin.Android.Volley\n" +
			"";
		mono.android.Runtime.register ("Volley.RequestQueue+IRequestFinishedListenerImplementor, Xamarin.Android.Volley", RequestQueue_RequestFinishedListenerImplementor.class, __md_methods);
	}


	public RequestQueue_RequestFinishedListenerImplementor ()
	{
		super ();
		if (getClass () == RequestQueue_RequestFinishedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Volley.RequestQueue+IRequestFinishedListenerImplementor, Xamarin.Android.Volley", "", this, new java.lang.Object[] {  });
	}


	public void onRequestFinished (com.android.volley.Request p0)
	{
		n_onRequestFinished (p0);
	}

	private native void n_onRequestFinished (com.android.volley.Request p0);

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
