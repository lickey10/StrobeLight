using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour {

	/// <summary>
	/// Singleton
	/// </summary>
	public static SoundEffectsHelper Instance;
	
	public AudioClip explosionSound;
	public AudioClip playerShotSound;
	public AudioClip enemyShotSound;
	public AudioClip gameOverSound;
	public AudioClip youWonSound;
	public AudioClip levelCompleteSound;
	public AudioClip levelEndedSound;
	public AudioClip levelLockedSound;
	public AudioClip levelStartedSound;
	
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of SoundEffectsHelper!");
		}
		Instance = this;
	}
	
	public void MakeExplosionSound()
	{
		MakeSound(explosionSound);
	}
	
	public void MakePlayerShotSound()
	{
		MakeSound(playerShotSound);
	}
	
	public void MakeEnemyShotSound()
	{
		MakeSound(enemyShotSound);
	}

	public void MakeGameOverSound()
	{
		MakeSound(gameOverSound);
	}

	public void MakeYouWonSound()
	{
		MakeSound(youWonSound);
	}

	public void MakeLevelCompleteSound()
	{
		MakeSound(levelCompleteSound);
	}

	public void MakeLevelEndedSound()
	{
		MakeSound(levelEndedSound);
	}

	/// <summary>
	/// Play a given sound
	/// </summary>
	/// <param name="originalClip"></param>
	private void MakeSound(AudioClip originalClip)
	{
		// As it is not 3D audio clip, position doesn't matter.
		AudioSource.PlayClipAtPoint(originalClip, transform.position);
	}
	
	public void MakeLevelStartedSound()
	{
		MakeSound(levelStartedSound);
	}

	public void MakeLevelLockedSound()
	{
		MakeSound(levelLockedSound);
	}
}
