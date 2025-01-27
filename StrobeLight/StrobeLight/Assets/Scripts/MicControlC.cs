﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(UISlider))]
public class MicControlC : MonoBehaviour
{

    public enum micActivation
    {
        HoldToSpeak,
        PushToSpeak,
        ConstantSpeak
    }

    public float sensitivity = 100;
    public float ramFlushSpeed = 5;//The smaller the number the faster it flush's the ram, but there might be performance issues...
    [Range(0, 100)]
    public float sourceVolume = 100;//Between 0 and 100
    public bool GuiSelectDevice = true;
    public micActivation micControl;
    public UIPanel uiPanel;
    //
    public string selectedDevice { get; private set; }
    public float loudness { get; private set; } //dont touch
    public GameObject[] Effects;
    public GameObject[] EffectTargets;
                                                //
    private bool micSelected = false;
    private float ramFlushTimer;
    private int amountSamples = 256; //increase to get better average, but will decrease performance. Best to leave it
    private int minFreq, maxFreq;
    private bool lightOn = false;
    private FlashControlsHelperClass flashControlHelperClass;
    private float threshold = 5;
    private UISlider soundLevelSlider;
    public bool ConstantStrobeLight = false;
    private UICheckbox cbConstantStrobe;

    void Start()
    {
        GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
        GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
        selectedDevice = Microphone.devices[0].ToString();
        micSelected = true;
        GetMicCaps();
        soundLevelSlider = GameObject.FindGameObjectWithTag("soundLevelSlider").GetComponent<UISlider>();
        cbConstantStrobe = GameObject.FindGameObjectWithTag("constantStrobeCheckbox").GetComponent<UICheckbox>();
        cbConstantStrobe.isChecked = ConstantStrobeLight;

#if UNITY_ANDROID
        flashControlHelperClass = FlashControlsHelperClass.Instance;
#endif
    }

    void OnGUI()
    {
        MicDeviceGUI((Screen.width / 2) - 150, (Screen.height / 2) - 75, 300, 100, 10, -300);
        if (Microphone.IsRecording(selectedDevice))
        {
            ramFlushTimer += Time.fixedDeltaTime;
            RamFlush();
        }
    }

    void OnSliderChange(float val)
    {
        threshold = val*10;

        if (val == 10)
            threshold = val * 10;
    }

    void OnActivate(bool isActive)
    {
        ConstantStrobeLight = isActive;
    }

    void StartGame()
    {
        //SceneManager.LoadScene("main");
        uiPanel.alpha = 0;
    }

    IEnumerator strobeTheLight(float secondsToWait)
    {
        lightOn = true;
#if UNITY_ANDROID
        flashControlHelperClass.ToggleLight();
        flashControlHelperClass.ToggleLight();
        StartCoroutine(showEffect());

        //print(Time.time);
        yield return new WaitForSeconds(secondsToWait);
        //print(Time.time);
        
#endif
        lightOn = false;
    }

    IEnumerator showEffect()
    {
        ArrayList effectGameObjects = new ArrayList();

        foreach (GameObject target in EffectTargets)
        {
            GameObject temp = (GameObject)Instantiate(Effects[Random.Range(0, Effects.Length - 1)], target.transform.position, Quaternion.identity);
            effectGameObjects.Add(temp);
        }

        yield return new WaitForSeconds(2.0f);

        foreach (GameObject target in effectGameObjects)
        {
            Destroy(target);
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

    public void MicDeviceGUI(float left, float top, float width, float height, float buttonSpaceTop, float buttonSpaceLeft)
    {
        if (Microphone.devices.Length > 1 && GuiSelectDevice == true || micSelected == false)//If there is more than one device, choose one.
            for (int i = 0; i < Microphone.devices.Length; ++i)
                if (GUI.Button(new Rect(left + ((width + buttonSpaceLeft) * i), top + ((height + buttonSpaceTop) * i), width, height), Microphone.devices[i].ToString()))
                {
                    StopMicrophone();
                    selectedDevice = Microphone.devices[i].ToString();
                    GetMicCaps();
                    StartMicrophone();
                    micSelected = true;
                }
        if (Microphone.devices.Length < 2 && micSelected == false)
        {//If there is only 1 decive make it default
            selectedDevice = Microphone.devices[0].ToString();
            GetMicCaps();
            micSelected = true;
        }
    }

    public void GetMicCaps()
    {
        Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
        if ((minFreq + maxFreq) == 0)//These 2 lines of code are mainly for windows computers
            maxFreq = 44100;
    }

    public void StartMicrophone()
    {
        GetComponent<AudioSource>().clip = Microphone.Start(selectedDevice, true, 10, maxFreq);//Starts recording
        while (!(Microphone.GetPosition(selectedDevice) > 0)) { } // Wait until the recording has started
        GetComponent<AudioSource>().Play(); // Play the audio source!
    }

    public void StopMicrophone()
    {
        GetComponent<AudioSource>().Stop();//Stops the audio
        Microphone.End(selectedDevice);//Stops the recording of the device	
    }

    void Update()
    {
        GetComponent<AudioSource>().volume = (sourceVolume / 100);
        loudness = GetAveragedVolume() * sensitivity * (sourceVolume / 10);

        soundLevelSlider.sliderValue = loudness/10;

        if(ConstantStrobeLight)
            StartCoroutine(strobeTheLight(1-soundLevelSlider.sliderValue));
        if(loudness > threshold)
            StartCoroutine(strobeTheLight(0));

        //Hold To Speak!!
        if (micControl == micActivation.HoldToSpeak)
        {
            if (Microphone.IsRecording(selectedDevice) && Input.GetKey(KeyCode.T) == false)
                StopMicrophone();
            //
            if (Input.GetKeyDown(KeyCode.T)) //Push to talk
                StartMicrophone();
            //
            if (Input.GetKeyUp(KeyCode.T))
                StopMicrophone();
            //
        }
        //Push To Talk!!
        if (micControl == micActivation.PushToSpeak)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (Microphone.IsRecording(selectedDevice))
                    StopMicrophone();

                else if (!Microphone.IsRecording(selectedDevice))
                    StartMicrophone();
            }
            //
        }
        //Constant Speak!!
        if (micControl == micActivation.ConstantSpeak)
            if (!Microphone.IsRecording(selectedDevice))
                StartMicrophone();
        //
        if (Input.GetKeyDown(KeyCode.G))
            micSelected = false;
    }

    private void RamFlush()
    {
        if (ramFlushTimer >= ramFlushSpeed && Microphone.IsRecording(selectedDevice))
        {
            StopMicrophone();
            StartMicrophone();
            ramFlushTimer = 0;
        }
    }

    float GetAveragedVolume()
    {
        float[] data = new float[amountSamples];
        float a = 0;
        GetComponent<AudioSource>().GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / amountSamples;
    }
}