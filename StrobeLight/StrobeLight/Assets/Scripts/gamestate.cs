/*
        Unity 3D: Game State Manager Script Source for State Manager
       
    Copyright 2012 FIZIX Digital Agency
    http://www.fizixstudios.com
       
        For more information see the tutorial at:
    http://www.fizixstudios.com/labs/do/view/id/unity-game-state-manager
       
       
    Notes:
        This script is a C# game state manager for Unity 3D; you should review the gamestart.cs
        script to help understand how to implement game states.
*/

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class gamestate : MonoBehaviour {
	
	// Declare properties
	private static gamestate instance;
	private int activeLevel = 1;                     // Active level
	private int levelProgress = 1;					//the max level the have achieved
	private int numberOfLevels = 1;						//the number of levels that exist in the game
	private int score = 0;							//the current game score
	private int highScore = 0;						//the highScore for the game since install
	private int lives = 3;							//the number of lives the player has
	private int health = 1;
	private bool gameRunning = false;				//whether the game is actually being played or not
	private int bannerHeight = 50; 				//the height of the banner to be displayed
	private bool gamePaused = false;


	private int activeLevel_default = 1;                     // Active level
	private int levelProgress_default = 1;					//the max level the have achieved
	private int numberOfLevels_default = 1;						//the number of levels that exist in the game
	private int score_default = 0;							//the current game score
	private int highScore_default = 0;						//the highScore for the game since install
	private int lives_default = 3;							//the number of lives the player has
	private int health_default = 1;
	private bool gameRunning_default = false;

	public bool IsGameWon = false;


	//	private string name;                            // Characters name
	//	private int maxHP;                                      // Max HP
	//	private int maxMP;                                      // Map MP
	//	private int hp;                                         // Current HP
	//	private int mp;                                         // Current MP
	//	private int str;                                        // Characters Strength
	//	private int vit;                                        // Characters Vitality
	//	private int dex;                                        // Characters Dexterity
	//	private int exp;                                        // Characters Experience Points

	BinaryFormatter bf = new BinaryFormatter();	
	
	// ---------------------------------------------------------------------------------------------------
	// gamestate()
	// ---------------------------------------------------------------------------------------------------
	// Creates an instance of gamestate as a gameobject if an instance does not exist
	// ---------------------------------------------------------------------------------------------------
	public static gamestate Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("gamestate").AddComponent<gamestate> ();
			}
			
			return instance;
		}
	} 


	
	// Sets the instance to null when the application quits
	public void OnApplicationQuit()
	{
		instance = null;
	}
	// ---------------------------------------------------------------------------------------------------
	
	
	// ---------------------------------------------------------------------------------------------------
	// startState()
	// ---------------------------------------------------------------------------------------------------
	// Creates a new game state
	// ---------------------------------------------------------------------------------------------------
	public void StartState()
	{
		print ("Creating a new game state");
		
		// Set default properties:
		StartState(numberOfLevels,lives,levelProgress,activeLevel,health,highScore);
	
		//		name = "My Character";
		//		maxHP = 250;
		//		maxMP = 60;
		//		hp = maxHP;
		//		mp = maxMP;
		//		str = 6;
		//		vit = 5;
		//		dex = 7;
		//		exp = 0;

	}

	public void StartState(int numOfLevels, int numLives, int newLevelProgress, int newActiveLevel, int newHealth, int newHighScore)
	{
		lives = numLives;
		activeLevel = newActiveLevel;
		numberOfLevels = numOfLevels;
		levelProgress = newLevelProgress;
		health = newHealth;
		highScore = newHighScore;

		//set defaults
		activeLevel_default = newActiveLevel;                     // Active level
		levelProgress_default = newLevelProgress;					//the max level the have achieved
		numberOfLevels_default = numOfLevels;						//the number of levels that exist in the game
		score_default = 0;							//the current game score
		highScore_default = newHighScore;						//the highScore for the game since install
		lives_default = numLives;							//the number of lives the player has
		health_default = newHealth;

		Application.LoadLevel("LevelMenu");
	}

	public void Reset()
	{
		lives = lives_default;
		//activeLevel = activeLevel_default;
		numberOfLevels = numberOfLevels_default;
		health = health_default;
		score = score_default;
		gameRunning = gameRunning_default;
		IsGameWon =false;
	}

	void OnGUI()
	{

	}

	public bool SetGamePaused(bool pauseGame)
	{
		if (pauseGame)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
		
		gamePaused = pauseGame;
		
		return gamePaused;
	}

	public bool GetGamePaused()
	{
		return gamePaused;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// getLevel()
	// ---------------------------------------------------------------------------------------------------
	// Returns the currently active level
	// ---------------------------------------------------------------------------------------------------
	public int getActiveLevel()
	{
		return activeLevel;
	}
	
	/// <summary>
	/// Gets the max level that is unlocked
	/// </summary>
	/// <returns>The max level.</returns>
	public int getLevelProgress()
	{
		return levelProgress;
	}

	public int SetLevelProgress(int newLevelProgress)
	{
		return levelProgress = newLevelProgress;
	}

	public void SetBannerHeight(int newBannerHeight)
	{
		bannerHeight = newBannerHeight;
	}

	public int GetBannerHeight()
	{
		return bannerHeight;
	}
	
	/// <summary>
	/// Gets the level count for the game
	/// </summary>
	/// <returns>The level count.</returns>
	public int GetNumberOfLevels()
	{
		return numberOfLevels;
	}

	public int SetNumberOfLevels(int numLevels)
	{
		return numberOfLevels = numLevels;
	}
	
	// ---------------------------------------------------------------------------------------------------
	// setLevel()
	// ---------------------------------------------------------------------------------------------------
	// Sets the currently active level to a new value
	// ---------------------------------------------------------------------------------------------------
	public void setActiveLevel(int newLevel)
	{
		// Set activeLevel to newLevel
		activeLevel = newLevel;

		if(activeLevel > numberOfLevels) //they won the game
		{
			IsGameWon = true;
			activeLevel = numberOfLevels;
		}
		
		if(activeLevel > levelProgress)
			levelProgress = activeLevel;
	}
	
	/// <summary>
	/// Gets the score.
	/// </summary>
	/// <returns>The score.</returns>
	public int getScore()
	{
		return score;
	}

	public int SetScore(int newScore)
	{
		if(newScore > highScore)
			SetHighScore(newScore);

		return score = newScore;
	}

	/// <summary>
	/// Gets the high score.
	/// </summary>
	/// <returns>The high score.</returns>
	public int getHighScore()
	{
		return highScore;
	}

	public int SetHighScore(int newHighScore)
	{
		return highScore = newHighScore;
	}
	
	/// <summary>
	/// Gets the lives.
	/// </summary>
	/// <returns>The lives.</returns>
	public int getLives()
	{
		return lives;
	}

	public int SetLives(int newLives)
	{
		return lives = newLives;
	}

	public int SetHealth(int newHealth)
	{
		return health = newHealth;
	}

	public int GetHealth()
	{
		return health;
	}

	public bool IsGameRunning()
	{
		return gameRunning;
	}

	public void SetGameRunning(bool GameRunning)
	{
		gameRunning = GameRunning;
	}

	/// <summary>
	/// Save this instance to player prefs
	/// </summary>
	public void Save(string playerPrefKey)
	{
		string message = "";

		try 
		{
			HappyPlane.GameStateObject serializableObject = new HappyPlane.GameStateObject();
			serializableObject.activeLevel = getActiveLevel();
			serializableObject.gameRunning = IsGameRunning();
			serializableObject.health = GetHealth();
			serializableObject.highScore = getHighScore();
			serializableObject.lives = getLives();
			serializableObject.levelProgress = getLevelProgress();
			serializableObject.numberOfLevels = GetNumberOfLevels();
			serializableObject.score = getScore();

			System.IO.MemoryStream memoryStream = new MemoryStream();
			
			bf.Serialize(memoryStream, serializableObject);
			string tmp = System.Convert.ToBase64String(memoryStream.ToArray());
			PlayerPrefs.SetString(playerPrefKey,tmp);
		} 
		catch (Exception ex) 
		{
			message = ex.Message;
		}
	}

	public void Load(string playerPrefKey)
	{
		string message = "";

		try 
		{
			string tmp = PlayerPrefs.GetString(playerPrefKey, string.Empty);

			if(tmp != string.Empty)
			{
				MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(tmp));
				
				HappyPlane.GameStateObject gameObject = (HappyPlane.GameStateObject)bf.Deserialize(memoryStream);

				gamestate.Instance.setActiveLevel(activeLevel_default);                     // Active level
				gamestate.Instance.SetLevelProgress(gameObject.levelProgress);					//the max level the have achieved
				//gamestate.Instance.SetNumberOfLevels(gameObject.numberOfLevels);						//the number of levels that exist in the game
				gamestate.Instance.SetScore(score_default);							//the current game score
				gamestate.Instance.SetHighScore(gameObject.highScore);						//the highScore for the game since install
				gamestate.Instance.SetLives(lives_default);							//the number of lives the player has
				gamestate.Instance.SetHealth(health_default);
				gamestate.Instance.SetGameRunning(gameRunning_default);
			}
		} 
		catch (Exception ex) 
		{
			message = ex.Message;
		}
	}


}