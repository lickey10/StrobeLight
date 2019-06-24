using UnityEngine;
using System.Collections;

public class FlashControl : MonoBehaviour {

	private bool isLightOn = false;
	private FlashControlsHelperClass flashControlHelperClass;
    string microphoneName = "";
    bool lightOn = false;
    float soundLevel = 0;
    float threshold = 6;
    MicControlC micControl;

    void Start()
	{
        micControl = GetComponent<MicControlC>();

#if UNITY_ANDROID
        flashControlHelperClass = FlashControlsHelperClass.Instance;

        //MicInput micInput = new MicInput();

        //foreach (string device in Microphone.devices) {
        //    if (device != null)
        //        microphoneName = device;

        //    Debug.Log("Name: " + device);
        //}
#endif
    }

    void Update()
    {
        if (micControl.loudness > threshold)//flash the light
        {
            
            StartCoroutine(strobeTheLight());
        }
        
    }

    void OnGUI()
    {
        if (micControl.loudness > threshold)//flash the light
        {
            var style = new GUIStyle("label");
            style.fontSize = 80;

            GUI.Label(new Rect(175, 0, 400, 400), "soundLevel:" + micControl.loudness, style);
        }
    }

        void OnMouseDown()
	{
		GetComponent<Renderer>().material.color = Color.red;

		try 
		{
#if UNITY_ANDROID
			flashControlHelperClass.ToggleLight();
            
#endif
        }
        catch (System.Exception ex) 
		{
			Debug.Log(ex.Message);
		}
	}

	void OnMouseUp()
	{
		GetComponent<Renderer>().material.color = Color.blue; //C#
	}

    IEnumerator strobeTheLight()
    {
        lightOn = true;
        flashControlHelperClass.ToggleLight();
        print(Time.time);
        yield return new WaitForSeconds(0f);
        print(Time.time);
        flashControlHelperClass.ToggleLight();
        lightOn = false;
    }
}
