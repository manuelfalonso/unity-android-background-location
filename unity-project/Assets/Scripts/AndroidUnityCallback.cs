using System;
using UnityEngine;

class AndroidUnityCallback : AndroidJavaProxy
{
    public AndroidUnityCallback() : base("com.hfugames.servicelib.UnityCallback") { }

    public Action<string> OnNewLocation;

    public void onSuccess(string data)
    {
        Debug.Log("ENTER callback onSuccess: " + data);
        OnNewLocation?.Invoke(data);
    }
}
