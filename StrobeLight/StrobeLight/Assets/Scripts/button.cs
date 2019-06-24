using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {
	public Texture2D ButtonBackground;
	int X = (Screen.width - 240 ) / 2;
	int Y = (Screen.height + 120) / 2;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI()
	{
		if(ButtonBackground != null)
		{

			if (
				GUI.Button(
				// Center in X, 1/3 of the height in Y
				new Rect(
				X,
				Y,
				240,
				120
				)
				,ButtonBackground, new GUIStyle()
				)
				)
			{
				if(!gamestate.Instance.IsGameRunning())
				{
					//the game isn't running so reset variables
					gamestate.Instance.Reset();
					Application.LoadLevel("logo");
				}
				else if(!gamestate.Instance.IsGameWon)
				{
					// start the game
					Application.LoadLevel("level"+ gamestate.Instance.getActiveLevel());
				}
				else //they won the game
				{
					gamestate.Instance.SetGameRunning(false);
					Application.LoadLevel("youWon");
				}
			}
		}
	}

}
