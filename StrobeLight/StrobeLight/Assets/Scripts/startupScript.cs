using UnityEngine;
using System.Collections;

public class startupScript : MonoBehaviour {
	public Texture logo;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
//		if(VariableManager.Instance == null)
//			VariableManager.Instantiate(this);

//		if(!VariableManager.Instance.GameStarted)
//		{
//			Time.timeScale = 0; 
//			const int buttonWidth = 120;
//			const int buttonHeight = 90;
//
////			GUI.color = Color.green;
////
////			//logo
////			GUI.DrawTexture(new Rect(Screen.width / 2 - 150,
////			                         30,
////			                         400,
////			                         200), logo, ScaleMode.ScaleToFit, true, 0.0F);
//
//
//			GUI.color = Color.white;
//			if (
//				GUI.Button(
//				// Center in X, 1/3 of the height in Y
//				new Rect(
//				Screen.width / 2 - (buttonWidth / 2),
//				(1 * Screen.height / 3) - (buttonHeight / 2),
//				buttonWidth,
//				buttonHeight
//				),
//				"Start!!"
//				)
//				)
//			{
//					Time.timeScale = 1;
//					VariableManager.Instance.GameStarted = true;
//			}
//		}


	}
}
