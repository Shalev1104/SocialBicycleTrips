package mono.com.android.volley;


public class Response_ErrorListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.android.volley.Response.ErrorListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onErrorResponse:(Lcom/android/volley/VolleyError;)V:GetOnErrorResponse_Lcom_android_volley_VolleyError_Handler:Volley.Response/IErrorListenerInvoker, Xamarin.Android.Volley\n" +
			"";
		mono.android.Runtime.register ("Volley.Response+IErrorListenerImplementor, Xamarin.Android.Volley", Response_ErrorListenerImplementor.class, __md_methods);
	}


	public Response_ErrorListenerImplementor ()
	{
		super ();
		if (getClass () == Response_ErrorListenerImplementor.class)
			mono.android.TypeManager.Activate ("Volley.Response+IErrorListenerImplementor, Xamarin.Android.Volley", "", this, new java.lang.Object[] {  });
	}


	public void onErrorResponse (com.android.volley.VolleyError p0)
	{
		n_onErrorResponse (p0);
	}

	private native void n_onErrorResponse (com.android.volley.VolleyError p0);

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
