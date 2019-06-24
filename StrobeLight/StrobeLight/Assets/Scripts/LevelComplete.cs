using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour {
	public GUIStyle customGuiStyle;
	public Sprite[] Backgrounds;

	// Use this for initialization
	void Start () {
		customGuiStyle = new GUIStyle();
		customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
		customGuiStyle.active.textColor = Color.red; // not working
		customGuiStyle.hover.textColor = Color.blue; // not working
		customGuiStyle.normal.textColor = Color.green;
		customGuiStyle.fontSize = 50;
		customGuiStyle.stretchWidth = true; // ---
		customGuiStyle.stretchHeight = true; // not working, since backgrounds aren't showing
		customGuiStyle.alignment = TextAnchor.MiddleCenter;

		setBackgroundForLevel();

		gamestate.Instance.setActiveLevel(gamestate.Instance.getActiveLevel()+1);
	}

	void setBackgroundForLevel()
	{
		GameObject.FindGameObjectWithTag("background").GetComponent<SpriteRenderer>().sprite = Backgrounds[gamestate.Instance.getActiveLevel() - 1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		int logoX = (Screen.width - 500 ) / 2;
		int logoY = (Screen.height - 300) / 2;
		int buttonWidth = 120;
		int buttonHeight = 60;

		//logo
		//drop shadow
		customGuiStyle.normal.textColor = Color.black;
		GUI.Box(new Rect( logoX+3, logoY+3, 450, 30 ), "Level Complete!" ,customGuiStyle);
		
		customGuiStyle.normal.textColor = Color.green;
		GUI.Box(new Rect( logoX, logoY, 450, 30 ), "Level Complete!" ,customGuiStyle);
	}
}
