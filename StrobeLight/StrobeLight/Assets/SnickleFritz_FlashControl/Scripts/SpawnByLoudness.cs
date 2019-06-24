using UnityEngine;
using System.Collections;

public class SpawnByLoudness : MonoBehaviour {
	
	public GameObject audioInputObject;
	public float threshold = 1.0f;
	public GameObject objectToSpawn;
	MicrophoneInput micIn;
    private FlashControlsHelperClass flashControlHelperClass;

    void Start() {
		//if (objectToSpawn == null)
		//	Debug.LogError("You need to set a prefab to Object To Spawn -parameter in the editor!");
		if (audioInputObject == null)
			audioInputObject = GameObject.Find("AudioInputObject");

		micIn = (MicrophoneInput) audioInputObject.GetComponent("MicrophoneInput");
	}
	
	void Update () {
		float l = micIn.loudness;
		if (l != 0)
		{
            //Vector3 scale = new Vector3(l,l,l);
            //GameObject newObject = (GameObject)Instantiate(objectToSpawn, this.transform.position, Quaternion.identity);
            //newObject.transform.localScale += scale;

            flashControlHelperClass.ToggleLight();
        }
	}

    void OnGUI()
    {
        var style = new GUIStyle("label");
        style.fontSize = 50;

        if(micIn != null)
            GUI.Label(new Rect(0, 0, 400, 400), "micIn.loudness:" + micIn.loudness, style);
    }
}