using UnityEngine;
using System.Collections;

public class LevelMenu : MonoBehaviour {
	public GUIStyle customGuiStyle;
	public Texture2D Border;
	public Texture2D Lock;
	public int MaxColumns = 3;
	public Vector2 scrollPosition = Vector2.zero;
	public Vector2 touchPosition = Vector2.zero;
	public Sprite[] Backgrounds;

	GUIContent buttonContent = new GUIContent();
	int buttonPadding = 4;

	int groupWidth = 500;
	int groupHeight = 260;
	int buttonWidth = 120;
	int buttonHeight = 120;
	int numRows = 0;

	// Use this for initialization
	void Start () {
		gamestate.Instance.SetNumberOfLevels(15);

		customGuiStyle = new GUIStyle();
		
		customGuiStyle.font = (Font)Resources.Load("Fonts/advlit");
		customGuiStyle.active.textColor = Color.red; // not working
		customGuiStyle.hover.textColor = Color.blue; // not working
		customGuiStyle.normal.textColor = Color.green;
		customGuiStyle.fontSize = 50;
		
		
		//		customGuiStyle.border.left = border; // not working, since backgrounds aren't showing
		//		customGuiStyle.border.right = border; // ---
		//		customGuiStyle.border.top = border; // ---
		//		customGuiStyle.border.bottom = border; // ---
		
		customGuiStyle.stretchWidth = true; // ---
		customGuiStyle.stretchHeight = true; // not working, since backgrounds aren't showing
		
		customGuiStyle.alignment = TextAnchor.MiddleCenter;

		buttonContent.image = Border;

		//the number of columns or the number of levels. which ever is less times button width plus padding
		if(gamestate.Instance.GetNumberOfLevels() < MaxColumns)
			groupWidth = (buttonWidth + buttonPadding) * gamestate.Instance.GetNumberOfLevels();
		else
			groupWidth = (buttonWidth + buttonPadding) * MaxColumns;

		numRows = (gamestate.Instance.GetNumberOfLevels()+MaxColumns-1)/MaxColumns;

		groupHeight = buttonHeight * 2;

		//set background
		GameObject.FindGameObjectWithTag("background").GetComponent<SpriteRenderer>().sprite = Backgrounds[gamestate.Instance.getActiveLevel() - 1];
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		//mouse controls
		//dragging
		if((Input.GetAxis("Mouse Y") != 0) && (Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetButton("Fire3")))
		{
			if(Input.GetAxis("Mouse Y") > 0)
				scrollPosition.y = scrollPosition.y - 8;
			else
				scrollPosition.y = scrollPosition.y + 8;
		}

		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if(touch.phase == TouchPhase.Began)
			{
				touchPosition = touch.position;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if(touchPosition.y > touch.position.y)
					scrollPosition.y = scrollPosition.y + 8;
				else
					scrollPosition.y = scrollPosition.y - 8;

			}
			else if (touch.phase == TouchPhase.Ended)
			{
				touchPosition = touch.position;
			}
		}

	}

	void OnGUI()
	{
		var screenWidth = Screen.width;
		var screenHeight = Screen.height;

		//var groupX = ( screenWidth - groupWidth ) / 2;
		var groupX = ( screenWidth - groupWidth ) / 2;
		var groupY = ( screenHeight - groupHeight ) / 2;

		GUI.BeginGroup(new Rect( groupX , groupY- buttonHeight, groupWidth, groupHeight ) );
		//GUI.backgroundColor = Color.blue;

		//logo
		//drop shadow
		customGuiStyle.normal.textColor = Color.black;
		GUI.Box(new Rect( 3, 3, 450, 30 ), "Happy Plane" ,customGuiStyle);
		
		customGuiStyle.normal.textColor = Color.green;
		GUI.Box(new Rect( 0, 0, 450, 30 ), "Happy Plane" ,customGuiStyle);

		//levels
		//drop shadow
		customGuiStyle.normal.textColor = Color.black;
		GUI.Box(new Rect( 3, 43, 450, 30 ), "Levels" ,customGuiStyle);

		customGuiStyle.normal.textColor = Color.green;
		GUI.Box(new Rect( 0, 40, 450, 30 ), "Levels" ,customGuiStyle);

		GUI.EndGroup();

		scrollPosition = GUI.BeginScrollView(new Rect( groupX , groupY, groupWidth+20, buttonHeight * 2 + buttonPadding), scrollPosition, new Rect(groupX , groupY, groupWidth, numRows * buttonHeight));

		//start of the button group
		GUI.BeginGroup(new Rect( groupX, groupY, groupWidth, buttonHeight * numRows ) );

		customGuiStyle.normal.textColor = Color.blue;
		GUI.Box(new Rect( 0, 0, groupWidth, buttonHeight * numRows ), "" );

		GUI.skin.button.normal.background = (Texture2D)buttonContent.image;
		GUI.skin.button.hover.background = (Texture2D)buttonContent.image;
		GUI.skin.button.active.background = (Texture2D)buttonContent.image;

		customGuiStyle.fontSize = 50;
		customGuiStyle.alignment = TextAnchor.MiddleCenter;

		for(int x = 0; x < numRows; x++)
		{
			// Begin a Row
			GUILayout.BeginHorizontal();

			// Use a for loop to create the row contents
			for (int y = 0; (x*MaxColumns) + y < gamestate.Instance.GetNumberOfLevels(); y++)
			{
				buttonContent.text = ((x*MaxColumns) + y+1).ToString();

				if(((x*MaxColumns) + y+1) > gamestate.Instance.getLevelProgress())//locked
				{
					customGuiStyle.normal.textColor = Color.gray;
					GUI.DrawTexture(new Rect(y*buttonWidth+buttonPadding,x*buttonHeight+buttonPadding,buttonWidth,buttonHeight),Border);


					if (GUI.Button(new Rect(y*buttonWidth+buttonPadding,x*buttonHeight+buttonPadding,buttonWidth-5,buttonHeight-5),((x*MaxColumns)+(y+1)).ToString(),customGuiStyle))
						SoundEffectsHelper.Instance.MakeLevelLockedSound();

					//draw lock
					GUI.DrawTexture(new Rect((y*buttonWidth+buttonPadding)+15,(x*buttonHeight+buttonPadding)+buttonHeight/1.5f,buttonWidth/4f,buttonHeight/4f),Lock);

				}
				else //unlocked
				{
					customGuiStyle.normal.textColor = Color.blue;
					GUI.DrawTexture(new Rect(y*buttonWidth+buttonPadding,x*buttonHeight+buttonPadding,buttonWidth,buttonHeight),Border);

					if (GUI.Button(new Rect(y*buttonWidth+buttonPadding,x*buttonHeight+buttonPadding,buttonWidth-5,buttonHeight-5),((x*MaxColumns)+(y+1)).ToString(),customGuiStyle))
					{
						int selectedLevel = (x*MaxColumns)+(y+1);
						gamestate.Instance.setActiveLevel(selectedLevel);
						Application.LoadLevel("level"+ selectedLevel.ToString());
						SoundEffectsHelper.Instance.MakeLevelStartedSound();
					}
				}

				if(y + 1 >= MaxColumns)
					break;
			}

			// End a Row
			GUILayout.EndHorizontal();

			if(x*MaxColumns > gamestate.Instance.GetNumberOfLevels())
				break;
		}

		GUI.EndGroup();

		GUI.EndScrollView();
	}
}
