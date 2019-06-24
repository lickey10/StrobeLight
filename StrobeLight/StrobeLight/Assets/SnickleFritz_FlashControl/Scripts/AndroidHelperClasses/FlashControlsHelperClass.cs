using UnityEngine;
using System.Collections;

public class FlashControlsHelperClass : MonoBehaviour {

	private static FlashControlsHelperClass _instance;

	public AndroidJavaObject FlashControls;

	public static FlashControlsHelperClass Instance
	{
		get
		{
			if(_instance == null)
			{
				//this makes Unity add the camera permissions to the AndroidManifest.xml so we can access the flash
				WebCamTexture wct;

				_instance = new FlashControlsHelperClass();
			}

			return _instance;
		}
	}

	public FlashControlsHelperClass()
	{
		FlashControls = new AndroidJavaObject("com.snicklefritz.cameraplugin.FlashControls");
	}

	public void ToggleLight()
	{
		FlashControls.Call("ToggleLight");
	}

	public void LightOff()
	{
		FlashControls.Call("LightOff");
	}

	public void LightOn()
	{
		FlashControls.Call("LightOn");
	}

	void OnApplicationQuit()
	{
		_instance = null;
	}
}
